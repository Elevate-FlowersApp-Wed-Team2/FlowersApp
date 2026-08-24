using FlowersApp.Notification.Domain.Enums;

namespace FlowersApp.Notification.Domain.Entities
{
    public class Notification:BaseEntity
    {
        public NotificationType Type { get; set; }

        public NotificationSource Source { get; set; }

        public string? Payload { get; set; }

        public NotificationStatus Status { get; set; }

        public ICollection<NotificationTranslation> Translations { get; set; }
            = new List<NotificationTranslation>();

        public ICollection<NotificationDelivery> Deliveries { get; set; }
            = new List<NotificationDelivery>();
    }
}
