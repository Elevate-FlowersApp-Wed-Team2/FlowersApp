using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.AspNetCore.Identity;

namespace FlowersApp.Auth.Features.Auth.Queries.VerifyPassword;

public record VerifyPasswordQuery(
    AppUser? User,
    DriverApplication? Application,
    string Password
) : IQuery<bool>;

public class VerifyPasswordQueryHandler : IQueryHandler<VerifyPasswordQuery, bool>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private static readonly string DummyHash = new PasswordHasher<AppUser>().HashPassword(new AppUser(), "DummyPassword123!");

    public VerifyPasswordQueryHandler(
        UserManager<AppUser> userManager,
        IPasswordHasher<AppUser> passwordHasher)
    {
        _userManager = userManager;
        _passwordHasher = passwordHasher;
    }

    public async Task<RequestResult<bool>> Handle(VerifyPasswordQuery request, CancellationToken cancellationToken)
    {
        if (request.User is not null)
        {
            var isValid = await _userManager.CheckPasswordAsync(request.User, request.Password);
            return RequestResult<bool>.succeeded(isValid, ResultCode.LoginSuccessful);
        }

        if (request.Application is not null && !string.IsNullOrEmpty(request.Application.HashedPassword))
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(
                new AppUser(),
                request.Application.HashedPassword,
                request.Password
            );

            var isValid = verificationResult == PasswordVerificationResult.Success ||
                          verificationResult == PasswordVerificationResult.SuccessRehashNeeded;

            return RequestResult<bool>.succeeded(isValid, ResultCode.LoginSuccessful);
        }

        // Constant-time dummy verification for non-existent accounts to prevent timing enumeration attacks
        _passwordHasher.VerifyHashedPassword(new AppUser(), DummyHash, request.Password);

        return RequestResult<bool>.succeeded(false, ResultCode.InvalidCredentials);
    }
}
