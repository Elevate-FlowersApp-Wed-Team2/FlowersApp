using FlowerApp.Auth.Features.Login;
using FlowerApp.Auth.Features.Logout;

namespace FlowerApp.Auth.Common.Extensions
{
    public static class EndpointExtensions
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapLoginEndpoint();
            app.MapGuestLoginEndpoint();
            app.MapLogoutEndpoint();
        }
    }
}
