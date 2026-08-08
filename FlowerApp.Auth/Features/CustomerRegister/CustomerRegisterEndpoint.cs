using MediatR;

namespace FlowerApp.Auth.Features.CustomerRegister
{
    public static class CustomerRegisterEndpoint
    {
        public static void MapCustomerRegisterEndpoint(
            this WebApplication app)
        {
            app.MapPost("/auth/register",
                async (
                    CustomerRegisterCommand command,
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
