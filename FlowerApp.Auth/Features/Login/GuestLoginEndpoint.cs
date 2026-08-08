using MediatR;

namespace FlowerApp.Auth.Features.Login
{
    public static class GuestLoginEndpoint
    {
        public static void MapGuestLoginEndpoint(this WebApplication app)
        {
            app.MapPost("/auth/guest",
                async (
                    IMediator mediator) =>
                {
                    var result = await mediator.Send(
                        new GuestLoginCommand());

                    return Results.Json(
                        result,
                        statusCode: result.StatusCode
                    );
                })
                .WithTags("Auth")
                .AllowAnonymous(); ;
        }
    }
}