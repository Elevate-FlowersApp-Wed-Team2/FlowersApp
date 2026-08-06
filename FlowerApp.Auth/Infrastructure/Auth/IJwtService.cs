using FlowerApp.Auth.Common.Enums;
using FlowerApp.Auth.Domain;

namespace FlowerApp.Auth.Infrastructure.Auth
{
    public interface IJwtService
    {
        string GenerateAccessToken(ApplicationUser user,string role,DriverStatus? driverStatus = null);
        RefreshToken GenerateRefreshToken();
    }
}
