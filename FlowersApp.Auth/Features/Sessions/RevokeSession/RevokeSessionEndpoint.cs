using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using System.Net;

namespace FlowersApp.Auth.Features.Sessions.RevokeSession;

public class RevokeSessionEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(Endpoints.Users.RevokeSession, async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new RevokeSessionCommand(id), cancellationToken);
            return result.Code switch
            {
                ResultCode.SessionRevokedSuccessfully => Results.Ok(
                    ApiResponse<object>.Success(null, HttpStatusCode.OK, result.Message)),

                ResultCode.SessionNotFound => Results.NotFound(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty, HttpStatusCode.NotFound)),

                ResultCode.UserNotFound => Results.NotFound(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty, HttpStatusCode.NotFound)),

                _ => Results.BadRequest(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty))
            };
        })
        .RequireAuthorization()
        .Produces<ApiResponse<object>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("RevokeSession")
        .WithTags("Users");
    }
}
