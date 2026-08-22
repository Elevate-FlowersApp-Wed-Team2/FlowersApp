namespace FlowersApp.Notification.Domain.Enums
{
    public enum DeliveryStatus
    {
        Pending = 1,
        Sending = 2,
        Sent = 3,
        Failed = 4,
        InvalidToken = 5
    }
}
