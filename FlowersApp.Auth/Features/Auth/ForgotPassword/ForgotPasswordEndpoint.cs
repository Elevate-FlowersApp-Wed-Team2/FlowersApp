using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.Auth.ForgotPassword;

public class ForgotPasswordEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(Endpoints.Auth.ForgotPassword, async (
            [FromBody] ForgotPasswordCommand request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request, cancellationToken);
            return result.Code switch
            {
                ResultCode.PasswordResetOtpSent => Results.Ok(
                    ApiResponse<object>.Success(null, HttpStatusCode.OK, result.Message)),

                ResultCode.OtpResendTooSoon => Results.Json(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty, HttpStatusCode.TooManyRequests),
                    statusCode: StatusCodes.Status429TooManyRequests),

                _ => Results.BadRequest(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty))
            };
        })
        .AllowAnonymous()
        .Produces<ApiResponse<object>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status429TooManyRequests)
        .WithName("ForgotPassword")
        .WithTags("Auth");
    }
}
