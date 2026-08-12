using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.Customer.Logout
{
    public record LogoutRequest(string? RefreshToken = null);

    public class LogoutEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost(Endpoints.Users.Logout, async (
                [FromBody(EmptyBodyBehavior = Microsoft.AspNetCore.Mvc.ModelBinding.EmptyBodyBehavior.Allow)] LogoutRequest? body,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = new LogoutCommand(body?.RefreshToken);
                var result = await mediator.Send(command, cancellationToken);

                return result.Code switch
                {
                    ResultCode.LoggedOutSuccessfully => Results.Ok(
                        ApiResponse<object>.Success(null, HttpStatusCode.OK, result.Message)),

                    ResultCode.UserNotFound => Results.NotFound(
                        ApiResponse<object>.Failure(result.Message ?? string.Empty, HttpStatusCode.NotFound)),

                    _ => Results.BadRequest(
                        ApiResponse<object>.Failure(result.Message ?? string.Empty))
                };
            })
            .RequireAuthorization()
            .Accepts<LogoutRequest>("application/json")
            .Produces<ApiResponse<object>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<object>>(StatusCodes.Status401Unauthorized)
            .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
            .WithName("Logout")
            .WithTags("Users");
        }
    }
}
