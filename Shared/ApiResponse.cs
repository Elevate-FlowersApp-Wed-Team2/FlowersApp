using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; }
        public string? Message { get; }
        public T? Data { get; }
        public List<string>? Errors { get; }

        public ApiResponse(bool isSuccess, T? data = default, string? message = null, List<string>? errors = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
            Errors = errors;
        }

        public static ApiResponse<T> Success(T data, string? message = null)
            => new(true, data, message);
        public static ApiResponse<T>Success(string? message = null)
        => new ApiResponse<T>(true, default, message);
        public static ApiResponse<T> Failure(string message, List<string>? errors = null)
            => new(false, default, message, errors);
    }

    public class ApiResponse
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
        public List<string>? Errors { get; init; }

        public static ApiResponse Ok(string? message = null)
            => new() { Success = true, Message = message };

        public static ApiResponse Fail(string? message = null, List<string>? errors = null)
            => new() { Success = false, Message = message, Errors = errors };
    }


}
