using FlowersApp.Notification.Domain.Entities;
using FlowersApp.Notification.Infrastructure.Persistence;
using FlowersApp.Notification.Shared.Interfaces;
using FlowersApp.Notification.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Notification.Features.Devices.RegisterDevice;

public class RegisterDeviceHandler : ICommandHandler<RegisterDeviceCommand, Guid>
{
    private readonly NotificationDbContext _context;
    private readonly ILogger<RegisterDeviceHandler> _logger;

    public RegisterDeviceHandler(NotificationDbContext context, ILogger<RegisterDeviceHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<RequestResult<Guid>> Handle(RegisterDeviceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registering device: UserId={UserId}, DeviceId={DeviceId}, Platform={Platform}",
            request.UserId, request.DeviceId, request.Platform);

        var now = DateTime.UtcNow;

        // Token Rotation: deactivate old installations with the same FcmToken on a different device or user
        var existingTokenInstallations = await _context.DeviceInstallations
            .Where(d => d.FcmToken == request.FcmToken && (d.UserId != request.UserId || d.DeviceId != request.DeviceId))
            .ToListAsync(cancellationToken);

        foreach (var oldInstallation in existingTokenInstallations)
        {
            _logger.LogInformation("Deactivating old device installation {OldId} due to token rotation", oldInstallation.Id);
            oldInstallation.IsActive = false;
        }

        // Find existing device installation by UserId & DeviceId
        var device = await _context.DeviceInstallations
            .FirstOrDefaultAsync(d => d.UserId == request.UserId && d.DeviceId == request.DeviceId, cancellationToken);

        if (device != null)
        {
            _logger.LogInformation("Updating existing device installation {DeviceId} for user {UserId}", request.DeviceId, request.UserId);
            device.FcmToken = request.FcmToken;
            device.Platform = request.Platform;
            device.Language = request.Language;
            device.AppVersion = request.AppVersion;
            device.IsActive = true;
            device.LastSeenAt = now;
        }
        else
        {
            _logger.LogInformation("Creating new device installation {DeviceId} for user {UserId}", request.DeviceId, request.UserId);
            device = new DeviceInstallation
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                DeviceId = request.DeviceId,
                FcmToken = request.FcmToken,
                Platform = request.Platform,
                Language = request.Language,
                AppVersion = request.AppVersion,
                IsActive = true,
                LastSeenAt = now
            };

            await _context.DeviceInstallations.AddAsync(device, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return RequestResult<Guid>.Succeeded(device.Id, ResultCode.DeviceRegisteredSuccessfully, "Device registered successfully.");
    }
}
