using System.Text.Json;
using FlowersApp.Notification.Domain.Entities;
using FlowersApp.Notification.Domain.Enums;
using FlowersApp.Notification.Infrastructure.Firebase;
using FlowersApp.Notification.Infrastructure.Persistence;
using FlowersApp.Notification.Shared.Interfaces;
using FlowersApp.Notification.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Notification.Features.Notifications.SendSystemNotification;

public class SendSystemNotificationHandler : ICommandHandler<SendSystemNotificationCommand, Guid>
{
    private readonly NotificationDbContext _context;
    private readonly IFcmService _fcmService;
    private readonly ILogger<SendSystemNotificationHandler> _logger;

    public SendSystemNotificationHandler(
        NotificationDbContext context,
        IFcmService fcmService,
        ILogger<SendSystemNotificationHandler> logger)
    {
        _context = context;
        _fcmService = fcmService;
        _logger = logger;
    }

    public async Task<RequestResult<Guid>> Handle(SendSystemNotificationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing System Notification: UserId={UserId}, Type={Type}", request.UserId, request.Type);

        var activeDevices = await _context.DeviceInstallations
            .Where(d => d.UserId == request.UserId && d.IsActive)
            .ToListAsync(cancellationToken);

        var notification = new Domain.Entities.Notification
        {
            Id = Guid.NewGuid(),
            Type = request.Type,
            Source = NotificationSource.System,
            Payload = request.Payload,
            Status = NotificationStatus.Processing
        };

        foreach (var t in request.Translations)
        {
            notification.Translations.Add(new NotificationTranslation
            {
                Id = Guid.NewGuid(),
                NotificationId = notification.Id,
                Language = t.Language.ToLowerInvariant(),
                Title = t.Title,
                Body = t.Body
            });
        }

        await _context.Notifications.AddAsync(notification, cancellationToken);

        if (!activeDevices.Any())
        {
            _logger.LogWarning("No active devices found for UserId={UserId}. Notification created with Completed status.", request.UserId);
            notification.Status = NotificationStatus.Completed;
            await _context.SaveChangesAsync(cancellationToken);
            return RequestResult<Guid>.Succeeded(notification.Id, ResultCode.NoActiveDevicesFound, "No active devices found for user, but notification recorded.");
        }

        Dictionary<string, string>? dataPayload = null;
        if (!string.IsNullOrWhiteSpace(request.Payload))
        {
            try
            {
                dataPayload = JsonSerializer.Deserialize<Dictionary<string, string>>(request.Payload);
            }
            catch
            {
                dataPayload = new Dictionary<string, string> { { "payload", request.Payload } };
            }
        }
        dataPayload ??= new Dictionary<string, string>();
        dataPayload["notificationId"] = notification.Id.ToString();
        dataPayload["notificationType"] = request.Type.ToString();

        var now = DateTime.UtcNow;

        foreach (var device in activeDevices)
        {
            var translation = notification.Translations.FirstOrDefault(t => t.Language.Equals(device.Language, StringComparison.OrdinalIgnoreCase))
                           ?? notification.Translations.FirstOrDefault(t => t.Language == "en")
                           ?? notification.Translations.First();

            var delivery = new NotificationDelivery
            {
                Id = Guid.NewGuid(),
                NotificationId = notification.Id,
                DeviceInstallationId = device.Id,
                Status = DeliveryStatus.Pending,
                AttemptCount = 0
            };

            notification.Deliveries.Add(delivery);

            delivery.Status = DeliveryStatus.Sending;
            delivery.AttemptCount++;
            delivery.LastAttemptAt = now;

            var result = await _fcmService.SendNotificationAsync(
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

                _logger.LogWarning("Deactivating device {DeviceId} (ID: {InstallationId}) due to invalid FCM token.", device.DeviceId, device.Id);
                device.IsActive = false;
            }
            else
            {
                delivery.Status = DeliveryStatus.Failed;
                delivery.ErrorCode = result.ErrorCode;
                delivery.ErrorMessage = result.ErrorMessage;
            }
        }

        notification.Status = NotificationStatus.Completed;
        await _context.SaveChangesAsync(cancellationToken);

        return RequestResult<Guid>.Succeeded(notification.Id, ResultCode.NotificationSentSuccessfully, "System notification processed.");
    }
}
