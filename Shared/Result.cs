using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public string? ErrorField { get; }
        public int StatusCode { get; }
        public object? Data { get; }

        private Result(bool isSuccess, string? error, string? errorField, int statusCode, object? data)
        {
            IsSuccess = isSuccess;
            Error = error;
            ErrorField = errorField;
            StatusCode = statusCode;
            Data = data;
        }

        public static Result Success(object? data = null) => new(true, null, null, 200, data);
        public static Result Failure(string error, int statusCode = 400, string? field = null) =>
            new(false, error, field, statusCode, null);
    }
}
