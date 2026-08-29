using FlowersApp.Notification.Domain.Enums;
using FlowersApp.Notification.Shared.Interfaces;

namespace FlowersApp.Notification.Features.Devices.RegisterDevice;

public class RegisterDeviceCommand : ICommand<Guid>
{
    public Guid UserId { get; set; }
    public string DeviceId { get; set; } = null!;
    public string FcmToken { get; set; } = null!;
    public DevicePlatform Platform { get; set; }
    public string Language { get; set; } = "en";
    public string? AppVersion { get; set; }
}
