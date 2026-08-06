using MediatR;
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
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var command = new ChangePasswordCommand(
                    userId, body.CurrentPassword, body.NewPassword, body.ConfirmNewPassword);

                var result = await sender.Send(command);

                return result.IsSuccess
                    ? Results.Ok(new { message = "Password changed. Please log in again." })
                    : Results.Json(new { error = result.Error }, statusCode: result.StatusCode);
            })
                .RequireAuthorization()
                .WithName("ChangePassword")
                .WithTags("Account")
                .Produces(200)
                .Produces(400)
                .Produces(401);
        }
    }
}
