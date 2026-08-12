using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.Account.VerifyResetOtp
{
    public class VerifyResetOtpEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost(Endpoints.Users.VerifyResetOtp, async (
                [FromBody] VerifyResetOtpCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                return result.Code switch
                {
                    ResultCode.OtpSent => Results.Ok(
                        ApiResponse<VerifyResetOtpResponse>.Success(result.Result, HttpStatusCode.OK)),

                    ResultCode.OtpExpired or
                    ResultCode.OtpMaxAttemptsExceeded => Results.BadRequest(
                        ApiResponse<VerifyResetOtpResponse>.Failure(result.Message, HttpStatusCode.BadRequest)),

                    _ => Results.BadRequest(
                        ApiResponse<VerifyResetOtpResponse>.Failure(result.Message, HttpStatusCode.BadRequest))
                };
            })
            .Produces<ApiResponse<VerifyResetOtpResponse>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<VerifyResetOtpResponse>>(StatusCodes.Status400BadRequest)
            .WithName("VerifyResetOtp")
            .WithTags("Account");
        }
    }
}
