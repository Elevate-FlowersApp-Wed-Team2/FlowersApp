using FlowerApp.Auth.Common.Enums;

namespace FlowerApp.Auth.Common
{
    public  class ApiResponse<T>
    {
        public int StatusCode { get; init; }

        public string Message { get; init; } = string.Empty;

        public IReadOnlyCollection<ErrorCode>? Errors { get; init; }

        public PaginationResponse? Pagination { get; init; }

        public T? Data { get; init; }

        public static ApiResponse<T> Success(T data,string message = "Success",int statusCode = StatusCodes.Status200OK)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = null,
                Pagination = null
            };
        }

        public static ApiResponse<T> Failure( string message,IReadOnlyCollection<ErrorCode> errors,int statusCode)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Errors = errors,
                Pagination = null,
                Data = default
            };
        }
    }
}
