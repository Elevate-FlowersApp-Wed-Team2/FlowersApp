using System.Net;

namespace FlowersApp.Notification.Shared.Response;

public class ApiResponse<T>
{
    public bool Status { get; set; }
    public T? Data { get; set; } = default;
    public List<string> Errors { get; set; } = new();
    public HttpStatusCode Code { get; set; } = HttpStatusCode.OK;
    public string Message { get; set; } = string.Empty;

    public ApiResponse() { }

    public ApiResponse(bool success, T? value, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.OK, string message = "")
    {
        Status = success;
        Data = value;
        Errors = errors ?? new List<string>();
        Code = statusCode;
        Message = message;
    }

    public static ApiResponse<T> Success(T? value, HttpStatusCode statusCode = HttpStatusCode.OK, string message = "")
        => new ApiResponse<T>(true, value, new List<string>(), statusCode, message);

    public static ApiResponse<T> Failure(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest, string message = "")
        => new ApiResponse<T>(false, default, new List<string> { errorMessage }, statusCode, message);

    public static ApiResponse<T> Failure(List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest, string message = "")
        => new ApiResponse<T>(false, default, errors ?? new List<string>(), statusCode, message);
}
