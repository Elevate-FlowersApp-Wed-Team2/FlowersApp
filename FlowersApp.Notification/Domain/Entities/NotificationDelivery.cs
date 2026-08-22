using FlowersApp.Notification.Domain.Enums;

namespace FlowersApp.Notification.Domain.Entities
{
    public class NotificationDelivery:BaseEntity
    {
        public Guid NotificationId { get; set; }

        public Guid DeviceInstallationId { get; set; }

        public DeliveryStatus Status { get; set; }

        public int AttemptCount { get; set; }

        public string? ProviderMessageId { get; set; }

        public string? ErrorCode { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime? LastAttemptAt { get; set; }

        public DateTime? SentAt { get; set; }

        public Notification Notification { get; set; } = null!;

        public DeviceInstallation DeviceInstallation { get; set; } = null!;
    }
}
