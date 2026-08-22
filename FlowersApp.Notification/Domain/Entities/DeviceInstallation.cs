using FlowersApp.Notification.Domain.Enums;

namespace FlowersApp.Notification.Domain.Entities
{
    public class DeviceInstallation:BaseEntity
    {
        public Guid UserId { get; set; }

        public string DeviceId { get; set; } = null!;

        public string FcmToken { get; set; } = null!;

        public DevicePlatform Platform { get; set; }

        public string Language { get; set; } = null!;

        public string? AppVersion { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? LastSeenAt { get; set; }

        public ICollection<NotificationDelivery> NotificationDeliveries { get; set; }
            = new List<NotificationDelivery>();

    }
}
