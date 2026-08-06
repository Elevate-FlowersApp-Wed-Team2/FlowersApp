using FlowerApp.Auth.Features.Login;

namespace FlowerApp.Auth.Common.Extensions
{
    public static class EndpointExtensions
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapLoginEndpoint();
            
        }
    }
}
