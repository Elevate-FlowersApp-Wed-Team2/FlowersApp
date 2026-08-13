using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.Account.ForgetPassword
{
    public class ForgotPasswordEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost(Endpoints.Users.ForgotPassword, async (
                [FromBody] ForgotPasswordCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                return Results.Ok(
                    ApiResponse<ForgotPasswordResponse>.Success(result.Result, HttpStatusCode.OK));
            })
            .Produces<ApiResponse<ForgotPasswordResponse>>(StatusCodes.Status200OK)
            .WithName("ForgotPassword")
            .WithTags("Account");
        }
    }
}
