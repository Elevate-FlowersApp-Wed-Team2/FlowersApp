using MediatR;
using Shared;
using System.Security.Claims;

namespace FlowerApp.Auth.Features.Account.ChangePassword
{
    public record ChangePasswordRequest(string CurrentPassword, string NewPassword, string ConfirmNewPassword);

    public static class ChangePasswordEndpoint
    {
        public static void MapChangePasswordEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("/users/me/change-password", async (
                    ChangePasswordRequest body,
                    ClaimsPrincipal user,
                    ISender sender) =>
            {
                var nameIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(nameIdClaim) || !Guid.TryParse(nameIdClaim, out var userId))
                {
                    return Results.Unauthorized();
                }

                var command = new ChangePasswordCommand(
                    userId, body.CurrentPassword, body.NewPassword, body.ConfirmNewPassword);

                var result = await sender.Send(command);

                return result.IsSuccess
                ? Results.Ok(ApiResponse.Ok("Password changed successfully."))
                : result.ToProblemResult();

            })
                .RequireAuthorization()
                .WithName("ChangePassword")
                .WithTags("Account")
                .Produces<ApiResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status401Unauthorized);
                
               
        }
    }
}
