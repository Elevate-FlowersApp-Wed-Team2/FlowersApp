using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FlowersApp.Auth.Domain.Entities;

namespace FlowersApp.Auth.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public TokenResult GenerateTokens(AppUser user, IEnumerable<string> roles, string? driverStatus = null)
    {
        var jwtKey = _config["JwtSettings:Key"] ?? _config["JwtSettings:Secret"];
        if (string.IsNullOrEmpty(jwtKey))
            throw new InvalidOperationException("JWT key is not configured");

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

        foreach (var r in roles)
        {
            if (!string.IsNullOrEmpty(r))
                claims.Add(new Claim(ClaimTypes.Role, r));
        }
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

        return new TokenResult(accessToken, refreshTokenValue, expiresInSeconds);
    }
}
