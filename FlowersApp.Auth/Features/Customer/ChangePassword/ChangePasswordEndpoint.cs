using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace FlowersApp.Auth.Features.Customer.ChangePassword
{
    public record ChangePasswordRequest(string CurrentPassword, string NewPassword, string ConfirmNewPassword);

    public class ChangePasswordEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            
            app.MapPost(Endpoints.Customers.ChangePassword, async (
                [FromBody] ChangePasswordCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                return result.Code switch
                {
                    ResultCode.PasswordChangedSuccessfully => Results.Ok(
                        ApiResponse<object>.Success(null, HttpStatusCode.OK, result.Message)),

                    ResultCode.UserNotFound => Results.NotFound(
                        ApiResponse<object>.Failure(result.Message ?? string.Empty, HttpStatusCode.NotFound)),

                    ResultCode.CurrentPasswordIncorrect => Results.BadRequest(
                        ApiResponse<object>.Failure(result.Message ?? string.Empty)),

                    
                    _ => Results.BadRequest(
                        ApiResponse<object>.Failure(result.Message ?? string.Empty))
                };
            })
            .RequireAuthorization()
            .Produces<ApiResponse<object>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
            .WithName("ChangePassword")
            .WithTags("Users");
        }
    }
}
