namespace FlowersApp.Catalog.Shared.Response;

public class RequestResult<TResult>
{
    public TResult? Result { get; set; }
    public ResultCode Code { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }

    public static RequestResult<TResult> Failure(TResult result, ResultCode code)
    {
        return new RequestResult<TResult>
        {
            Code = code,
            Success = false,
            Result = result
        };
    }

    public static RequestResult<TResult> Failure(ResultCode code)
    {
        return new RequestResult<TResult>
        {
            Code = code,
            Success = false,
            Result = default
        };
    }

    public static RequestResult<TResult> succeeded(TResult result, ResultCode code)
    {
        return new RequestResult<TResult>
        {
            Code = code,
            Success = true,
            Result = result
        };
    }
}