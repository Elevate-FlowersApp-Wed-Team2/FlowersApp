namespace FlowersApp.Notification.Infrastructure.Firebase;

public interface IFcmService
{
    Task<FcmSendResult> SendNotificationAsync(
        string fcmToken,
        string title,
        string body,
        IReadOnlyDictionary<string, string>? data = null,
        CancellationToken cancellationToken = default);
}
