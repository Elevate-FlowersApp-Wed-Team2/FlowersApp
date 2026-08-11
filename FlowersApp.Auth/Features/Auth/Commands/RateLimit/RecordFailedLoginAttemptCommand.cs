using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;

namespace FlowersApp.Auth.Features.Auth.Commands.RateLimit;

public record RecordFailedLoginAttemptCommand(string Email) : ICommand<Unit>;

public class RecordFailedLoginAttemptCommandHandler : ICommandHandler<RecordFailedLoginAttemptCommand, Unit>
{
    private readonly ILoginRateLimiter _rateLimiter;

    public RecordFailedLoginAttemptCommandHandler(ILoginRateLimiter rateLimiter)
    {
        _rateLimiter = rateLimiter;
    }

    public async Task<RequestResult<Unit>> Handle(RecordFailedLoginAttemptCommand request, CancellationToken cancellationToken)
    {
        await _rateLimiter.RecordFailedAttemptAsync(request.Email, cancellationToken);
        return RequestResult<Unit>.succeeded(Unit.Value, ResultCode.InvalidCredentials);
    }
}
