using CustomerEntity = FlowersApp.Auth.Domain.Entities.Customer;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Features.Auth.UserLogin;

public class UserLoginHandler : ICommandHandler<UserLoginCommand, AuthResponse>
{
    private static readonly string DummyHash =
        new PasswordHasher<AppUser>().HashPassword(new AppUser(), "DummyPassword123!");

    private readonly UserManager<AppUser> _userManager;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly ILoginRateLimiter _rateLimiter;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserLoginHandler> _logger;

    public UserLoginHandler(
        UserManager<AppUser> userManager,
        IPasswordHasher<AppUser> passwordHasher,
        ITokenService tokenService,
        ILoginRateLimiter rateLimiter,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UserLoginHandler> logger)
    {
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _rateLimiter = rateLimiter;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<RequestResult<AuthResponse>> Handle(
        UserLoginCommand request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();
        var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        var deviceInfo = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();

        if (await _rateLimiter.IsRateLimitedAsync(email, ip, cancellationToken))
        {
            _logger.LogWarning("Customer login rate-limited for {Email}.", email);
            return RequestResult<AuthResponse>.Failure(ResultCode.TooManyFailedAttempts);
        }

        var normalizedEmail = _userManager.NormalizeEmail(email);
        var customer = await _userManager.Users
            .OfType<CustomerEntity>()
            .FirstOrDefaultAsync(c => c.NormalizedEmail == normalizedEmail, cancellationToken);

        var passwordValid = customer is not null
            ? await _userManager.CheckPasswordAsync(customer, request.Password)
            : DummyVerify(request.Password);

        if (customer is null || !passwordValid)
        {
            await _rateLimiter.RecordFailedAttemptAsync(email, ip, cancellationToken);
            _logger.LogWarning("Customer login failed with invalid credentials.");
            return RequestResult<AuthResponse>.Failure(ResultCode.InvalidCredentials);
        }

        var roles = await _userManager.GetRolesAsync(customer);
        if (!roles.Contains(UserRoles.Customer))
        {
            await _rateLimiter.RecordFailedAttemptAsync(email, ip, cancellationToken);
            return RequestResult<AuthResponse>.Failure(ResultCode.InvalidCredentials);
        }

        await _rateLimiter.ResetAttemptsAsync(email, ip, cancellationToken);

        var auth = await AuthTokenIssuer.IssueAsync(
            _tokenService,
            customer,
            UserRoles.Customer,
            applicationStatus: null,
            deviceInfo,
            ip,
            cancellationToken);

        _logger.LogInformation("Customer {UserId} logged in successfully.", customer.Id);
        return RequestResult<AuthResponse>.succeeded(auth, ResultCode.LoginSuccessful);
    }

    private bool DummyVerify(string password)
    {
        _passwordHasher.VerifyHashedPassword(new AppUser(), DummyHash, password);
        return false;
    }
}
