using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Infrastructure.Persistence;
using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Features.Auth.DriverLogin;

public class DriverLoginHandler : ICommandHandler<DriverLoginCommand, AuthResponse>
{
    private static readonly string DummyHash =
        new PasswordHasher<AppUser>().HashPassword(new AppUser(), "DummyPassword123!");

    private readonly UserManager<AppUser> _userManager;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly ILoginRateLimiter _rateLimiter;
    private readonly AppDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<DriverLoginHandler> _logger;

    public DriverLoginHandler(
        UserManager<AppUser> userManager,
        IPasswordHasher<AppUser> passwordHasher,
        ITokenService tokenService,
        ILoginRateLimiter rateLimiter,
        AppDbContext db,
        IHttpContextAccessor httpContextAccessor,
        ILogger<DriverLoginHandler> logger)
    {
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _rateLimiter = rateLimiter;
        _db = db;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<RequestResult<AuthResponse>> Handle(
        DriverLoginCommand request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();
        var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        var deviceInfo = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();

        if (await _rateLimiter.IsRateLimitedAsync(email, ip, cancellationToken))
        {
            _logger.LogWarning("Driver login rate-limited for {Email}.", email);
            return RequestResult<AuthResponse>.Failure(ResultCode.TooManyFailedAttempts);
        }

        var normalizedEmail = _userManager.NormalizeEmail(email);
        var driver = await _userManager.Users
            .OfType<Driver>()
            .FirstOrDefaultAsync(d => d.NormalizedEmail == normalizedEmail, cancellationToken);

        if (driver is not null)
        {
            var passwordValid = await _userManager.CheckPasswordAsync(driver, request.Password);
            if (!passwordValid)
            {
                await _rateLimiter.RecordFailedAttemptAsync(email, ip, cancellationToken);
                return RequestResult<AuthResponse>.Failure(ResultCode.InvalidCredentials);
            }

            await _rateLimiter.ResetAttemptsAsync(email, ip, cancellationToken);

            var applicationStatus = await ResolveDriverApplicationStatusAsync(driver, cancellationToken);

            if (string.IsNullOrWhiteSpace(applicationStatus) ||
                applicationStatus == DriverApplicationStatus.Pending.ToString())
            {
                return RequestResult<AuthResponse>.Failure(
                    AuthTokenIssuer.EmptyWithStatus(
                        UserRoles.Driver,
                        applicationStatus ?? DriverApplicationStatus.Pending.ToString()),
                    ResultCode.DriverAccountNotApproved);
            }

            if (applicationStatus == DriverApplicationStatus.Rejected.ToString())
            {
                return RequestResult<AuthResponse>.Failure(
                    AuthTokenIssuer.EmptyWithStatus(UserRoles.Driver, applicationStatus),
                    ResultCode.DriverApplicationRejected);
            }

            if (applicationStatus != DriverApplicationStatus.Approved.ToString())
            {
                return RequestResult<AuthResponse>.Failure(ResultCode.InvalidCredentials);
            }

            var auth = await AuthTokenIssuer.IssueAsync(
                _tokenService,
                driver,
                UserRoles.Driver,
                applicationStatus,
                deviceInfo,
                ip,
                cancellationToken);

            _logger.LogInformation("Driver {UserId} logged in successfully.", driver.Id);
            return RequestResult<AuthResponse>.succeeded(auth, ResultCode.LoginSuccessful);
        }

        var application = await _db.Applications
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower(), cancellationToken);

        var appPasswordValid = application is not null && !string.IsNullOrEmpty(application.HashedPassword)
            ? VerifyApplicationPassword(application.HashedPassword, request.Password)
            : DummyVerify(request.Password);

        if (application is null || !appPasswordValid)
        {
            await _rateLimiter.RecordFailedAttemptAsync(email, ip, cancellationToken);
            return RequestResult<AuthResponse>.Failure(ResultCode.InvalidCredentials);
        }

        await _rateLimiter.ResetAttemptsAsync(email, ip, cancellationToken);

        return application.Status switch
        {
            DriverApplicationStatus.Pending => RequestResult<AuthResponse>.Failure(
                AuthTokenIssuer.EmptyWithStatus(UserRoles.Driver, DriverApplicationStatus.Pending.ToString()),
                ResultCode.DriverAccountNotApproved),

            DriverApplicationStatus.Rejected => RequestResult<AuthResponse>.Failure(
                AuthTokenIssuer.EmptyWithStatus(UserRoles.Driver, DriverApplicationStatus.Rejected.ToString()),
                ResultCode.DriverApplicationRejected),

            // Approved applications require a Driver AppUser from admin onboarding.
            // Do not reveal that the application exists / password was correct.
            DriverApplicationStatus.Approved =>
                RequestResult<AuthResponse>.Failure(ResultCode.InvalidCredentials),

            _ => RequestResult<AuthResponse>.Failure(ResultCode.InvalidCredentials)
        };
    }

    private async Task<string?> ResolveDriverApplicationStatusAsync(
        Driver driver,
        CancellationToken cancellationToken)
    {
        if (driver.DriverApplicationId is Guid appId)
        {
            var status = await _db.Applications
                .AsNoTracking()
                .Where(a => a.Id == appId)
                .Select(a => (DriverApplicationStatus?)a.Status)
                .FirstOrDefaultAsync(cancellationToken);
            return status?.ToString();
        }

        var byEmail = await _db.Applications
            .AsNoTracking()
            .Where(a => a.Email == driver.Email)
            .OrderByDescending(a => a.Id)
            .Select(a => (DriverApplicationStatus?)a.Status)
            .FirstOrDefaultAsync(cancellationToken);

        return byEmail?.ToString();
    }

    private bool VerifyApplicationPassword(string hashedPassword, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(new AppUser(), hashedPassword, password);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }

    private bool DummyVerify(string password)
    {
        _passwordHasher.VerifyHashedPassword(new AppUser(), DummyHash, password);
        return false;
    }
}
