using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using System.Net;

namespace FlowersApp.Auth.Features.Sessions.ListSessions;

public class ListSessionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(Endpoints.Users.Sessions, async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new ListSessionsQuery(), cancellationToken);
            return result.Code switch
            {
                ResultCode.SessionsRetrievedSuccessfully => Results.Ok(
                    ApiResponse<IReadOnlyList<SessionDto>>.Success(result.Result, HttpStatusCode.OK, result.Message)),

                ResultCode.UserNotFound => Results.NotFound(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty, HttpStatusCode.NotFound)),

                _ => Results.BadRequest(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty))
            };
        })
        .RequireAuthorization()
        .Produces<ApiResponse<IReadOnlyList<SessionDto>>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("ListSessions")
        .WithTags("Users");
    }
}
