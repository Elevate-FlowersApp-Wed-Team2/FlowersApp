using FlowersApp.Auth.Features.Auth.Commands.GenerateAuthTokens;
using FlowersApp.Auth.Features.Auth.Commands.RateLimit;
using FlowersApp.Auth.Features.Auth.Queries.GetCustomerByEmail;
using FlowersApp.Auth.Features.Auth.Queries.VerifyPassword;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;

namespace FlowersApp.Auth.Features.Auth.UserLogin;

public class UserLoginOrchestratorHandler(IMediator mediator) : ICommandHandler<UserLoginOrchestrator, AuthResponse>
{
    private readonly IMediator _mediator = mediator;

    public async Task<RequestResult<AuthResponse>> Handle(UserLoginOrchestrator request, CancellationToken cancellationToken)
    {
        // 1. Check rate limit
        var rateLimitCheck = await _mediator.Send(new CheckLoginRateLimitQuery(request.Email), cancellationToken);
        if (rateLimitCheck.Result)
        {
            return RequestResult<AuthResponse>.Failure(ResultCode.TooManyFailedAttempts);
        }

        // 2. Query customer by email
        var customerResult = await _mediator.Send(new GetCustomerByEmailQuery(request.Email), cancellationToken);
        var customer = customerResult.Result;

        // 3. Verify password (or constant-time dummy check if customer is null)
        var passwordCheck = await _mediator.Send(new VerifyPasswordQuery(customer, null, request.Password), cancellationToken);

        if (customer is null || !passwordCheck.Result)
        {
            await _mediator.Send(new RecordFailedLoginAttemptCommand(request.Email), cancellationToken);
            return RequestResult<AuthResponse>.Failure(ResultCode.InvalidCredentials);
        }

        // 4. Reset rate limit counter on successful authentication
        await _mediator.Send(new ResetLoginRateLimitCommand(request.Email), cancellationToken);

        // 5. Generate Auth Tokens
        return await _mediator.Send(new GenerateAuthTokensCommand(customer, "Customer"), cancellationToken);
    }
}
