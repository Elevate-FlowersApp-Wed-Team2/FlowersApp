using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Infrastructure.Persistence;
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

    public Task<(string AccessToken, DateTime ExpiresAt)> GenerateAccessTokenAsync(
        AppUser user,
        string role,
        CancellationToken cancellationToken = default)
    {
        var secretKey = !string.IsNullOrWhiteSpace(_options.Key)
            ? _options.Key
            : "FlowersApp_Super_Secret_Jwt_Security_Key_2026!#$";

        var issuer = string.IsNullOrWhiteSpace(_options.Issuer) ? "FlowersApp" : _options.Issuer;
        var audience = string.IsNullOrWhiteSpace(_options.Audience) ? "FlowersAppClient" : _options.Audience;
        var durationMinutes = _options.DurationInMinutes > 0 ? _options.DurationInMinutes : 60;
        
        var expiresAt = DateTime.UtcNow.AddMinutes(durationMinutes);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.FullName ?? string.Empty),
            new(ClaimTypes.Role, role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessTokenString = tokenHandler.WriteToken(token);

        return Task.FromResult((accessTokenString, expiresAt));
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var refreshExpirationDays = _options.RefreshTokenExpirationDays > 0 ? _options.RefreshTokenExpirationDays : 30;

        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var tokenString = Convert.ToBase64String(randomNumber);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = tokenString,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshExpirationDays),
            RevokedAt = null
        };

        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync(cancellationToken);

        return refreshToken;
    }
}
