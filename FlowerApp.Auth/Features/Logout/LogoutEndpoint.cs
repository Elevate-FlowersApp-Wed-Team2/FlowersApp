using MediatR;
using System.Runtime.CompilerServices;

namespace FlowerApp.Auth.Features.Logout
{

    public static class LogoutEndpoint
    {
        public static void MapLogoutEndpoint(this WebApplication app)
        {
            app.MapPost("/auth/logout",
                async (                    
                    IMediator mediator) =>
                {
                    LogoutCommand command = new LogoutCommand();
                    var result = await mediator.Send(command);
                    return Results.Json(
                        result,
                        statusCode: result.StatusCode
                    );
                })
                .WithTags("Auth")
                .RequireAuthorization();
        }
    }
}
