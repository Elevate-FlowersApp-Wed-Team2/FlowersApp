using FluentValidation.Results;

namespace FlowersApp.Auth.Shared.Exceptions;

public class ValidationException : Exception
{
    public IReadOnlyList<ValidationFailure> Failures { get; }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
    {
        Failures = failures?.Where(f => f != null).ToList()
            ?? throw new ArgumentNullException(nameof(failures));
    }

    public ValidationException(ValidationResult validationResult)
        : this(validationResult?.Errors ?? throw new ArgumentNullException(nameof(validationResult)))
    {
    }
}