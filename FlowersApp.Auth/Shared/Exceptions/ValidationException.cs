using FlowersApp.Auth.Shared.Response;
using FluentValidation.Results;
using System.Net;

namespace FlowersApp.Auth.Shared.Exceptions;

public class ValidationException : Exception
{
    public ApiResponse<object> Failure { get; private set; } // Fixed spelling

    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Failure = ApiResponse<object>.Failure("One or more validation failures have occurred.");
        Failure.StatusCode = HttpStatusCode.BadRequest;
    }

    // Constructor for ValidationResult (what you have)
    public ValidationException(ValidationResult validationResult)
        : base("One or more validation failures have occurred.")
    {
        if (validationResult == null)
            throw new ArgumentNullException(nameof(validationResult));

        var errors = validationResult.Errors
            .Where(f => f != null)
            .Select(f => f.ErrorMessage)
            .ToList();

        Failure = ApiResponse<object>.Failure(errors);
        Failure.StatusCode = HttpStatusCode.BadRequest; // ← ADD THIS LINE
    }

    // NEW: Constructor for List<ValidationFailure> (what your behavior uses)
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
    {
        if (failures == null)
            throw new ArgumentNullException(nameof(failures));

        var errors = failures
            .Where(f => f != null)
            .Select(f => f.ErrorMessage)
            .ToList();

        Failure = ApiResponse<object>.Failure(errors);
        Failure.StatusCode = HttpStatusCode.BadRequest; // ← ADD THIS LINE
    }

    // NEW: Constructor for simple error messages
    public ValidationException(string errorMessage)
        : base("One or more validation failures have occurred.")
    {
        Failure = ApiResponse<object>.Failure(errorMessage);
        Failure.StatusCode = HttpStatusCode.BadRequest;
    }
}