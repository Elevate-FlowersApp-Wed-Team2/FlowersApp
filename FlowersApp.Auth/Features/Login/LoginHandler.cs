using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Infrastructure.Persistence;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using FlowersApp.Auth.Shared.Interfaces;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FlowersApp.Auth.Infrastructure.Services;
using FlowersApp.Auth.Features.RefreshTokens;
using System.Security.Claims;

namespace FlowersApp.Auth.Features.Login;

public class LoginHandler : ICommandHandler<LoginCommand, AuthResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _db;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _config;
    private readonly ILogger<LoginHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenService _tokenService;
    private readonly IMediator _mediator;

    public LoginHandler(
        UserManager<AppUser> userManager,
        AppDbContext db,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IMediator mediator,
        IConfiguration config,
        ILogger<LoginHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _db = db;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mediator = mediator;
        _config = config;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<RequestResult<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Rate limit key per email+ip
        var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
        var normalizedEmail = request.Email?.Trim().ToUpperInvariant() ?? string.Empty;
        var failKey = $"login:fail:{normalizedEmail}:{ip}";

        var attempts = await _redis.IncrementAsync(failKey);
        if (attempts == 1)
            await _redis.ExpireAsync(failKey, BLOCK_WINDOW);

        if (attempts > MAX_ATTEMPTS)
        {
            _logger.LogWarning("Login rate limited for {Email} from {IP}", request.Email, ip);
            return RequestResult<AuthResponse>.Failure(ResultCode.LoginRateLimited);
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        var passwordValid = false;
        if (user is not null)
        {
            passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        }

        if (user is null || !passwordValid)
        {
            _logger.LogInformation("Invalid login attempt for {Email} from {IP}", request.Email, ip);
            return RequestResult<AuthResponse>.Failure(ResultCode.InvalidEmailOrPassword);
        }

        // Successful - reset attempts
        await _redis.DeleteAsync(failKey);

        // get roles
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? string.Empty;

        // driver status if applicable
        string? driverStatus = null;
        if (user is Driver drv)
        {
            driverStatus = drv.DriverStatus.ToString();
        }

        // Generate tokens via token service
        TokenResult tokenResult;
        try
        {
            tokenResult = _tokenService.GenerateTokens(user, roles, driverStatus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate tokens for {Email}", request.Email);
            return RequestResult<AuthResponse>.Failure(ResultCode.LoginFailed);
        }

        // persist refresh token via command
        var saveCmd = new SaveRefreshTokenCommand
        {
            UserId = user.Id,
            Token = tokenResult.RefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };

        var saveResult = await _mediator.Send(saveCmd, cancellationToken);

        var response = new AuthResponse
        {
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken,
            ExpiresIn = tokenResult.ExpiresIn,
            Role = role,
            DriverStatus = driverStatus
        };

        return RequestResult<AuthResponse>.succeeded(response, ResultCode.LoginSuccessful);
    }
}
