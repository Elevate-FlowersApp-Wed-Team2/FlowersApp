using FirebaseAdmin.Messaging;

namespace FlowersApp.Notification.Infrastructure.Firebase;

public class FcmService : IFcmService
{
    private readonly ILogger<FcmService> _logger;

    public FcmService(ILogger<FcmService> logger)
    {
        _logger = logger;
    }

    public async Task<FcmSendResult> SendNotificationAsync(
        string fcmToken,
        string title,
        string body,
        IReadOnlyDictionary<string, string>? data = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fcmToken))
        {
            return FcmSendResult.InvalidToken("EMPTY_TOKEN", "FCM token cannot be null or empty.");
        }

        var message = new Message
        {
            Token = fcmToken,
            Notification = new FirebaseAdmin.Messaging.Notification
            {
                Title = title,
                Body = body
            },
            Data = data != null ? new Dictionary<string, string>(data) : null
        };

        try
        {
            var tokenPrefix = fcmToken.Length > 10 ? fcmToken[..10] + "..." : fcmToken;
            _logger.LogInformation("Sending FCM notification to token: {TokenPrefix}", tokenPrefix);

            var messageId = await FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);

            _logger.LogInformation("FCM notification sent successfully. MessageId: {MessageId}", messageId);

            return FcmSendResult.Success(messageId);
        }
        catch (FirebaseMessagingException ex)
        {
            _logger.LogWarning(ex, "Firebase Messaging Exception: MessagingErrorCode={ErrorCode}, ErrorCode={StatusCode}, Message={Message}", 
                ex.MessagingErrorCode, ex.ErrorCode, ex.Message);

            var isInvalidToken = ex.MessagingErrorCode == MessagingErrorCode.Unregistered ||
                                 ex.MessagingErrorCode == MessagingErrorCode.InvalidArgument ||
                                 ex.MessagingErrorCode == MessagingErrorCode.ThirdPartyAuthError ||
                                 ex.Message.Contains("invalid-registration-token", StringComparison.OrdinalIgnoreCase) ||
                                 ex.Message.Contains("registration-token-not-registered", StringComparison.OrdinalIgnoreCase);

            if (isInvalidToken)
            {
                return FcmSendResult.InvalidToken(ex.MessagingErrorCode?.ToString() ?? "INVALID_TOKEN", ex.Message);
            }

            return FcmSendResult.Failure(ex.MessagingErrorCode?.ToString() ?? "FCM_ERROR", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while sending FCM notification.");
            return FcmSendResult.Failure("INTERNAL_FCM_ERROR", ex.Message);
        }
    }
}
