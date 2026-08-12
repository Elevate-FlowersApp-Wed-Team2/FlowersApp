using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.Auth.VerifyOtp;

public class VerifyOtpEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(Endpoints.Auth.VerifyOtp, async (
            [FromBody] VerifyOtpCommand request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request, cancellationToken);
            return result.Code switch
            {
                ResultCode.OtpVerifiedSuccessfully => Results.Ok(
                    ApiResponse<VerifyOtpResponse>.Success(result.Result, HttpStatusCode.OK, result.Message)),

                ResultCode.OtpExpired => Results.BadRequest(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty)),

                ResultCode.OtpMaxAttemptsExceeded => Results.BadRequest(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty)),

                ResultCode.InvalidOrExpiredOtp => Results.BadRequest(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty)),

                _ => Results.BadRequest(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty))
            };
        })
        .AllowAnonymous()
        .Produces<ApiResponse<VerifyOtpResponse>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .WithName("VerifyOtp")
        .WithTags("Auth");
    }
}
