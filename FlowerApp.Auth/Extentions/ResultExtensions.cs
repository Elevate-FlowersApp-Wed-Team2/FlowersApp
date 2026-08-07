using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shared
{
    public static class ResultExtensions
    {
        public static IResult ToProblemResult(this Result result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException("Cannot convert a successful result to a problem result.");

            var statusCode = result.ErrorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            return Results.Problem(
                detail: result.Error,
                statusCode: statusCode,
                title: result.ErrorType switch
                {
                    ErrorType.Validation => "Validation Error",
                    ErrorType.NotFound => "Resource Not Found",
                    ErrorType.Conflict => "Conflict",
                    ErrorType.Unauthorized => "Unauthorized",
                    ErrorType.Forbidden => "Forbidden",
                    _ => "Server Error"
                });
        }
    }
}