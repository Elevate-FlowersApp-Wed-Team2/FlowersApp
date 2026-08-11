using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;

namespace FlowersApp.Auth.Features.Auth.Commands.RateLimit;

public record CheckLoginRateLimitQuery(string Email) : IQuery<bool>;

public class CheckLoginRateLimitQueryHandler : IQueryHandler<CheckLoginRateLimitQuery, bool>
{
    private readonly ILoginRateLimiter _rateLimiter;

    public CheckLoginRateLimitQueryHandler(ILoginRateLimiter rateLimiter)
    {
        _rateLimiter = rateLimiter;
    }

    public async Task<RequestResult<bool>> Handle(CheckLoginRateLimitQuery request, CancellationToken cancellationToken)
    {
        var isLimited = await _rateLimiter.IsRateLimitedAsync(request.Email, cancellationToken);
        return RequestResult<bool>.succeeded(isLimited, isLimited ? ResultCode.TooManyFailedAttempts : ResultCode.LoginSuccessful);
    }
}
