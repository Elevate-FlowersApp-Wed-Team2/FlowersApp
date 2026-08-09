using FlowerApp.Auth.Common.Enums;
using FlowerApp.Auth.Domain;

namespace FlowerApp.Auth.Infrastructure.Auth
{
    public interface IJwtService
    {
        int AccessTokenExpirationInSeconds { get; }
        string GenerateAccessToken(ApplicationUser user,string role, ApplicationDriverStatus? applicationStatus = null);
        string GenerateGuestAccessToken(Guid guestSessionId);
        (string RawToken, RefreshToken RefreshToken) GenerateRefreshToken();
    }
}