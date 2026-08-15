using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Infrastructure.Persistence;
using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FlowersApp.Auth.Infrastructure.Authentication;

public class JwtTokenService : ITokenService
{
    private readonly AppDbContext _db;
    private readonly JwtOptions _options;

    public JwtTokenService(AppDbContext db, IOptions<JwtOptions> options)
    {
        _db = db;
        _options = options.Value;
    }

    public Task<AccessTokenResult> GenerateAccessTokenAsync(
        AppUser user,
        string role,
        string? applicationStatus = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.Key))
            throw new InvalidOperationException("Jwt:Key is not configured.");

        var durationMinutes = _options.DurationInMinutes > 0 ? _options.DurationInMinutes : 15;
        var expiresAt = DateTime.UtcNow.AddMinutes(durationMinutes);
        var expiresInSeconds = (int)TimeSpan.FromMinutes(durationMinutes).TotalSeconds;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.FullName ?? string.Empty),
            new(ClaimTypes.Role, role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (role == UserRoles.Driver && !string.IsNullOrWhiteSpace(applicationStatus))
            claims.Add(new Claim(AuthClaimTypes.ApplicationStatus, applicationStatus));

        var key = Encoding.UTF8.GetBytes(_options.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            Issuer = string.IsNullOrWhiteSpace(_options.Issuer) ? "FlowersApp" : _options.Issuer,
            Audience = string.IsNullOrWhiteSpace(_options.Audience) ? "FlowersAppClient" : _options.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(new AccessTokenResult(tokenHandler.WriteToken(token), expiresAt, expiresInSeconds));
    }

    public async Task<RefreshTokenIssueResult> GenerateRefreshTokenAsync(
        Guid userId,
        Guid? familyId = null,
        string? deviceInfo = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default)
    {
        var refreshExpirationDays = _options.RefreshTokenExpirationDays > 0
            ? _options.RefreshTokenExpirationDays
            : 30;

        var rawBytes = RandomNumberGenerator.GetBytes(64);
        var rawToken = Convert.ToBase64String(rawBytes);

        var entity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = HashToken(rawToken),
            FamilyId = familyId ?? Guid.NewGuid(),
            DeviceInfo = deviceInfo,
            IpAddress = ipAddress,
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshExpirationDays),
            RevokedAt = null
        };

        _db.RefreshTokens.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return new RefreshTokenIssueResult(rawToken, entity);
    }

    public string HashToken(string rawToken)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
        return Convert.ToBase64String(hash);
    }
}
