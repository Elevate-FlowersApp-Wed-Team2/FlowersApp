namespace FlowersApp.Notification.Domain.Entities
{
    public class NotificationTranslation:BaseEntity
    {
        public Guid NotificationId { get; set; }

        public string Language { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Body { get; set; } = null!;

        public Notification Notification { get; set; } = null!;
    }
}
