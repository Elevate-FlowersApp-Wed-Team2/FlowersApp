using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.Account.ResetPassword
{
    public class ResetPasswordEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost(Endpoints.Users.ResetPassword, async (
                [FromBody] ResetPasswordOrchestrator command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                return result.Code switch
                {
                    ResultCode.PasswordResetSuccessfully => Results.Ok(
                        ApiResponse<ResetPasswordResponse>.Success(result.Result, HttpStatusCode.OK)),

                    ResultCode.ResetTokenInvalid or
                    ResultCode.UserNotFound => Results.BadRequest(
                        ApiResponse<ResetPasswordResponse>.Failure(result.Message, HttpStatusCode.BadRequest)),

                    _ => Results.BadRequest(
                        ApiResponse<ResetPasswordResponse>.Failure(result.Message, HttpStatusCode.BadRequest))
                };
            })
            .Produces<ApiResponse<ResetPasswordResponse>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<ResetPasswordResponse>>(StatusCodes.Status400BadRequest)
            .WithName("ResetPassword")
            .WithTags("Account");
        }
    }
}
