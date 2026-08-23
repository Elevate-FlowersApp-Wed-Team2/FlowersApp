using System.Text.Json;
using FlowersApp.Notification.Domain.Enums;
using FlowersApp.Notification.Infrastructure.Firebase;
using FlowersApp.Notification.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Notification.Infrastructure.BackgroundServices;

public class NotificationRetryBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NotificationRetryBackgroundService> _logger;
    private static readonly TimeSpan PollingInterval = TimeSpan.FromSeconds(30);
    private const int MaxAttempts = 3;
    private const int BatchSize = 50;

    public NotificationRetryBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<NotificationRetryBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NotificationRetryBackgroundService starting...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingDeliveriesAsync(stoppingToken);
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogError(ex, "Error occurred in NotificationRetryBackgroundService execution loop.");
            }

            await Task.Delay(PollingInterval, stoppingToken);
        }

        _logger.LogInformation("NotificationRetryBackgroundService stopping.");
    }

    private async Task ProcessPendingDeliveriesAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
        var fcmService = scope.ServiceProvider.GetRequiredService<IFcmService>();

        var pendingDeliveries = await context.NotificationDeliveries
            .Include(d => d.Notification)
                .ThenInclude(n => n.Translations)
            .Include(d => d.DeviceInstallation)
            .Where(d => (d.Status == DeliveryStatus.Pending || d.Status == DeliveryStatus.Failed)
                        && d.AttemptCount < MaxAttempts
                        && d.DeviceInstallation.IsActive)
            .OrderBy(d => d.CreatedAt)
            .Take(BatchSize)
            .ToListAsync(cancellationToken);

        if (!pendingDeliveries.Any())
        {
            return;
        }

        _logger.LogInformation("Retrying {Count} notification deliveries...", pendingDeliveries.Count);

        var now = DateTime.UtcNow;

        foreach (var delivery in pendingDeliveries)
        {
            var device = delivery.DeviceInstallation;
            var notification = delivery.Notification;

            if (device == null || !device.IsActive || notification == null)
            {
                delivery.Status = DeliveryStatus.Failed;
                delivery.ErrorMessage = "Associated device or notification invalid/inactive.";
                continue;
            }

            var translation = notification.Translations.FirstOrDefault(t => t.Language.Equals(device.Language, StringComparison.OrdinalIgnoreCase))
                           ?? notification.Translations.FirstOrDefault(t => t.Language == "en")
                           ?? notification.Translations.FirstOrDefault();

            if (translation == null)
            {
                delivery.Status = DeliveryStatus.Failed;
                delivery.ErrorMessage = "No translation found for delivery.";
                continue;
            }

            Dictionary<string, string>? dataPayload = null;
            if (!string.IsNullOrWhiteSpace(notification.Payload))
            {
                try
                {
                    dataPayload = JsonSerializer.Deserialize<Dictionary<string, string>>(notification.Payload);
                }
                catch
                {
                    dataPayload = new Dictionary<string, string> { { "payload", notification.Payload } };
                }
            }
            dataPayload ??= new Dictionary<string, string>();
            dataPayload["notificationId"] = notification.Id.ToString();

            delivery.Status = DeliveryStatus.Sending;
            delivery.AttemptCount++;
            delivery.LastAttemptAt = now;

            var result = await fcmService.SendNotificationAsync(
                device.FcmToken,
                translation.Title,
                translation.Body,
                dataPayload,
                cancellationToken);

            if (result.IsSuccess)
            {
                delivery.Status = DeliveryStatus.Sent;
                delivery.SentAt = DateTime.UtcNow;
                delivery.ProviderMessageId = result.MessageId;
            }
            else if (result.IsInvalidToken)
            {
                delivery.Status = DeliveryStatus.InvalidToken;
                delivery.ErrorCode = result.ErrorCode;
                delivery.ErrorMessage = result.ErrorMessage;

                _logger.LogWarning("Background retry: Deactivating device {DeviceId} (ID: {InstallationId}) due to invalid token.", device.DeviceId, device.Id);
                device.IsActive = false;
            }
            else
            {
                delivery.Status = DeliveryStatus.Failed;
                delivery.ErrorCode = result.ErrorCode;
                delivery.ErrorMessage = result.ErrorMessage;
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
