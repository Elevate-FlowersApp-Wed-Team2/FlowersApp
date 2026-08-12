using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.Auth.ResetPassword;

public class ResetPasswordEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(Endpoints.Auth.ResetPassword, async (
            [FromBody] ResetPasswordCommand request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request, cancellationToken);
            return result.Code switch
            {
                ResultCode.PasswordResetSuccessful => Results.Ok(
                    ApiResponse<object>.Success(null, HttpStatusCode.OK, result.Message)),

                ResultCode.InvalidOrExpiredResetToken => Results.BadRequest(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty)),

                ResultCode.UserNotFound => Results.NotFound(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty, HttpStatusCode.NotFound)),

                _ => Results.BadRequest(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty))
            };
        })
        .AllowAnonymous()
        .Produces<ApiResponse<object>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("ResetPassword")
        .WithTags("Auth");
    }
}
