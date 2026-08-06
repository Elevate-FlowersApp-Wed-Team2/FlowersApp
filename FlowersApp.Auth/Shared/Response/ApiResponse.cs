using System.Net;

namespace FlowersApp.Auth.Shared.Response;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Value { get; set; } = default;
    public List<string> Errors { get; set; } = new();
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public ApiResponse() { }
    public ApiResponse(bool success, T? value, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        IsSuccess = success;
        Value = value;
        Errors = errors ?? new List<string>();
        StatusCode = statusCode;
    }

    // Success response
    public static ApiResponse<T> Success(T? value, HttpStatusCode statusCode = HttpStatusCode.OK)
        => new ApiResponse<T>(true, value, new List<string>(), statusCode);

    // Failure with single error message
    public static ApiResponse<T> Failure(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new ApiResponse<T>(false, default, new List<string> { errorMessage }, statusCode);

    // Failure with multiple error messages
    public static ApiResponse<T> Failure(List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new ApiResponse<T>(false, default, errors ?? new List<string>(), statusCode);

}