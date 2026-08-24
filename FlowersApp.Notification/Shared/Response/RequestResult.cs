namespace FlowersApp.Notification.Shared.Response;

public class RequestResult<TResult>
{
    public TResult? Result { get; set; }
    public ResultCode Code { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    public static RequestResult<TResult> Failure(ResultCode code, string message = "")
    {
        return new RequestResult<TResult>
        {
            Code = code,
            Success = false,
            Result = default,
            Message = message
        };
    }

    public static RequestResult<TResult> Failure(TResult result, ResultCode code, string message = "")
    {
        return new RequestResult<TResult>
        {
            Code = code,
            Success = false,
            Result = result,
            Message = message
        };
    }

    public static RequestResult<TResult> Succeeded(TResult result, ResultCode code, string message = "")
    {
        return new RequestResult<TResult>
        {
            Code = code,
            Success = true,
            Result = result,
            Message = message
        };
    }
}
