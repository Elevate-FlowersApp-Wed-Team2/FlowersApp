using FluentValidation;
using MediatR;
using System.Globalization;

namespace FlowersApp.Auth.Shared.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> _validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // 1. Check if we have any validators
        if (!_validators.Any())
            return await next(); // No validators? Let the package through

        var context = new ValidationContext<TRequest>(request);

        // Step 3: Run all validators in parallel
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        // Step 4: Collect all validation errors
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        // 4. If we found problems, STOP and throw exception
        if (failures.Any())
        {
            Console.WriteLine($"[VALIDATION BEHAVIOR] CurrentUICulture={CultureInfo.CurrentUICulture} at throw time");
            throw new FlowersApp.Auth.Shared.Exceptions.ValidationException(failures); // ← JUST THROW, don't try to create response
        }

        // 5. No problems? Send to the actual handler
        return await next();

    }
}
