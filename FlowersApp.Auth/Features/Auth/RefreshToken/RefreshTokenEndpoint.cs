using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.Auth.RefreshToken;

public class RefreshTokenEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(Endpoints.Auth.RefreshToken, async (
            [FromBody] RefreshTokenCommand request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request, cancellationToken);
            return result.Code switch
            {
                ResultCode.TokenRefreshedSuccessfully => Results.Ok(
                    ApiResponse<AuthResponse>.Success(result.Result, HttpStatusCode.OK, result.Message)),

                ResultCode.InvalidRefreshToken => Results.Unauthorized(),

                ResultCode.RefreshTokenReuseDetected => Results.Unauthorized(),

                ResultCode.UserNotFound => Results.NotFound(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty, HttpStatusCode.NotFound)),

                _ => Results.BadRequest(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty))
            };
        })
        .AllowAnonymous()
        .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .WithName("RefreshToken")
        .WithTags("Auth");
    }
}
