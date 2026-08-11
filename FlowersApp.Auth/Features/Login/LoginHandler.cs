using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Infrastructure.Persistence;
using FlowersApp.Auth.Shared.Response;
using FlowersApp.Shared.Redis;
using MediatR;
using FlowersApp.Auth.Shared.Interfaces;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlowersApp.Auth.Features.Login;

public class LoginHandler : ICommandHandler<LoginCommand, AuthResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _db;
    private readonly IRedisCacheService _redis;
    private readonly IConfiguration _config;
    private readonly ILogger<LoginHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private const int MAX_ATTEMPTS = 5;
    private static readonly TimeSpan BLOCK_WINDOW = TimeSpan.FromMinutes(15);

    public LoginHandler(
        UserManager<AppUser> userManager,
        AppDbContext db,
        IRedisCacheService redis,
        IConfiguration config,
        ILogger<LoginHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _db = db;
        _redis = redis;
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

        // Generate tokens
        var jwtKey = _config["JwtSettings:Key"] ?? _config["JwtSettings:Secret"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            _logger.LogError("JWT key is not configured");
            return RequestResult<AuthResponse>.Failure(ResultCode.LoginFailed);
        }

        var keyBytes = Convert.FromBase64String(jwtKey);
        var signingKey = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var expiresInSeconds = int.TryParse(_config["JwtSettings:AccessTokenExpirySeconds"], out var s) ? s : 600;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
        };
        if (!string.IsNullOrEmpty(role))
            claims.Add(new Claim(ClaimTypes.Role, role));
        if (!string.IsNullOrEmpty(driverStatus))
            claims.Add(new Claim("driverStatus", driverStatus));

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddSeconds(expiresInSeconds),
            signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        // generate refresh token
        var refreshTokenValueBytes = new byte[64];
        RandomNumberGenerator.Fill(refreshTokenValueBytes);
        var refreshTokenValue = Convert.ToBase64String(refreshTokenValueBytes);
        var refresh = new Domain.Entities.RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            UserId = user.Id
        };
        _db.RefreshTokens.Add(refresh);
        await _db.SaveChangesAsync(cancellationToken);

        var response = new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            ExpiresIn = expiresInSeconds,
            Role = role,
            DriverStatus = driverStatus
        };

        return RequestResult<AuthResponse>.succeeded(response, ResultCode.LoginSuccessful);
    }
}
