using FlowerApp.Auth.Common.Enums;
using FlowerApp.Auth.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FlowerApp.Auth.Infrastructure.Auth
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;


        public JwtService(
            IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }

        public int AccessTokenExpirationInSeconds
                    => _jwtSettings.ExpirationMinutes * 60;

        public string GenerateAccessToken(ApplicationUser user, string role, DriverStatus? driverStatus = null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Role, role)
            };

            if (driverStatus is not null)
            {
                claims.Add(new Claim("DriverStatus", driverStatus.ToString()));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var signingCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (string RawToken, RefreshToken RefreshToken) GenerateRefreshToken()
        {
            var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var hashedToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));
            var refreshToken = new RefreshToken
            {
                Token = hashedToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            };
            return (rawToken, refreshToken);
        }
    }
}
