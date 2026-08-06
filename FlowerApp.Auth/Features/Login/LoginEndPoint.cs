using MediatR;

namespace FlowerApp.Auth.Features.Login
{
    public static class LoginEndpoint
    {
        public static void MapLoginEndpoint(this WebApplication app)
        {
            app.MapPost("/auth/login",
                async (
                    LoginCommand command,
                    IMediator mediator) =>
                {
                    var result = await mediator.Send(command);

                    return Results.Json(
                        result,
                        statusCode: result.StatusCode
                    );
                })
                .WithTags("Auth");
        }
    }
}
