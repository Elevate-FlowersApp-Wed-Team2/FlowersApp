namespace FlowersApp.Notification.Infrastructure.Firebase;

public class FcmSendResult
{
    public bool IsSuccess { get; set; }
    public string? MessageId { get; set; }
    public bool IsInvalidToken { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }

    public static FcmSendResult Success(string messageId) => new()
    {
        IsSuccess = true,
        MessageId = messageId
    };

    public static FcmSendResult InvalidToken(string errorCode, string errorMessage) => new()
    {
        IsSuccess = false,
        IsInvalidToken = true,
        ErrorCode = errorCode,
        ErrorMessage = errorMessage
    };

    public static FcmSendResult Failure(string errorCode, string errorMessage) => new()
    {
        IsSuccess = false,
        IsInvalidToken = false,
        ErrorCode = errorCode,
        ErrorMessage = errorMessage
    };
}
