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
        public string Error { get; }
        public ErrorType ErrorType { get; }

        protected Result(bool isSuccess, string error, ErrorType errorType)
        {
            IsSuccess = isSuccess;
            Error = error;
            ErrorType = errorType;
        }

        public static Result Success() => new(true, string.Empty, ErrorType.Failure);
        public static Result Failure(string error, ErrorType errorType = ErrorType.Failure)
            => new(false, error, errorType);
    }
    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(T? value, bool isSuccess, string? error, ErrorType errorType)
            : base(isSuccess, error, errorType)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new(value, true, null, ErrorType.None);

        public static new Result<T> Failure(string error, ErrorType errorType = ErrorType.Failure)
            => new(default, false, error, errorType);
    }
}

