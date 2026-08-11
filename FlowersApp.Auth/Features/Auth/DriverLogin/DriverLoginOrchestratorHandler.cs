using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Features.Auth.Commands.GenerateAuthTokens;
using FlowersApp.Auth.Features.Auth.Commands.RateLimit;
using FlowersApp.Auth.Features.Auth.Queries.GetDriverApplicationByEmail;
using FlowersApp.Auth.Features.Auth.Queries.GetDriverUserByEmail;
using FlowersApp.Auth.Features.Auth.Queries.VerifyPassword;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;

namespace FlowersApp.Auth.Features.Auth.DriverLogin;

public class DriverLoginOrchestratorHandler(IMediator mediator) : ICommandHandler<DriverLoginOrchestrator, AuthResponse>
{
    private readonly IMediator _mediator = mediator;

    public async Task<RequestResult<AuthResponse>> Handle(DriverLoginOrchestrator request, CancellationToken cancellationToken)
    {
        // 1. Check rate limit
        var rateLimitCheck = await _mediator.Send(new CheckLoginRateLimitQuery(request.Email), cancellationToken);
        if (rateLimitCheck.Result)
        {
            return RequestResult<AuthResponse>.Failure(ResultCode.TooManyFailedAttempts);
        }

        // 2. Query Driver AppUser by email
        var driverUserResult = await _mediator.Send(new GetDriverUserByEmailQuery(request.Email), cancellationToken);
        var driverUser = driverUserResult.Result;

        if (driverUser is not null)
        {
            var pwdCheck = await _mediator.Send(new VerifyPasswordQuery(driverUser, null, request.Password), cancellationToken);
            if (!pwdCheck.Result)
            {
                await _mediator.Send(new RecordFailedLoginAttemptCommand(request.Email), cancellationToken);
                return RequestResult<AuthResponse>.Failure(ResultCode.InvalidCredentials);
            }

            await _mediator.Send(new ResetLoginRateLimitCommand(request.Email), cancellationToken);
            return await _mediator.Send(new GenerateAuthTokensCommand(driverUser, "Driver", DriverApplicationStatus.Approved.ToString()), cancellationToken);
        }

        // 3. Query DriverApplication by email if Driver AppUser does not exist
        var applicationResult = await _mediator.Send(new GetDriverApplicationByEmailQuery(request.Email), cancellationToken);
        var application = applicationResult.Result;

        // Verify password hash against DriverApplication OR dummy check if application is null
        var appPwdCheck = await _mediator.Send(new VerifyPasswordQuery(null, application, request.Password), cancellationToken);

        if (application is null || !appPwdCheck.Result)
        {
            await _mediator.Send(new RecordFailedLoginAttemptCommand(request.Email), cancellationToken);
            return RequestResult<AuthResponse>.Failure(ResultCode.InvalidCredentials);
        }

        // 4. Reset rate limit counter on successful password verification
        await _mediator.Send(new ResetLoginRateLimitCommand(request.Email), cancellationToken);

        // 5. Evaluate Driver Application Status
        return application.Status switch
        {
            DriverApplicationStatus.Pending => RequestResult<AuthResponse>.Failure(
                new AuthResponse(
                    UserId: Guid.Empty,
                    Email: application.Email,
                    FullName: application.FullName,
                    Role: "Driver",
                    AccessToken: string.Empty,
                    RefreshToken: string.Empty,
                    AccessTokenExpiresAt: DateTime.MinValue,
                    DriverStatus: DriverApplicationStatus.Pending.ToString()
                ),
                ResultCode.DriverAccountNotApproved
            ),

            DriverApplicationStatus.Rejected => RequestResult<AuthResponse>.Failure(
                new AuthResponse(
                    UserId: Guid.Empty,
                    Email: application.Email,
                    FullName: application.FullName,
                    Role: "Driver",
                    AccessToken: string.Empty,
                    RefreshToken: string.Empty,
                    AccessTokenExpiresAt: DateTime.MinValue,
                    DriverStatus: DriverApplicationStatus.Rejected.ToString()
                ),
                ResultCode.DriverApplicationRejected
            ),

            DriverApplicationStatus.Approved => await _mediator.Send(
                new GenerateAuthTokensCommand(
                    new AppUser
                    {
                        Id = application.Id,
                        Email = application.Email,
                        FullName = application.FullName,
                        UserName = application.Email
                    },
                    "Driver",
                    DriverApplicationStatus.Approved.ToString()
                ),
                cancellationToken
            ),

            _ => RequestResult<AuthResponse>.Failure(ResultCode.InvalidCredentials)
        };
    }
}
