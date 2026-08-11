using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;

namespace FlowersApp.Auth.Features.Auth.Commands.RateLimit;

public record ResetLoginRateLimitCommand(string Email) : ICommand<Unit>;

public class ResetLoginRateLimitCommandHandler : ICommandHandler<ResetLoginRateLimitCommand, Unit>
{
    private readonly ILoginRateLimiter _rateLimiter;

    public ResetLoginRateLimitCommandHandler(ILoginRateLimiter rateLimiter)
    {
        _rateLimiter = rateLimiter;
    }

    public async Task<RequestResult<Unit>> Handle(ResetLoginRateLimitCommand request, CancellationToken cancellationToken)
    {
        await _rateLimiter.ResetAttemptsAsync(request.Email, cancellationToken);
        return RequestResult<Unit>.succeeded(Unit.Value, ResultCode.LoginSuccessful);
    }
}
