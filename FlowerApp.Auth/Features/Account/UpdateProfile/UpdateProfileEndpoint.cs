using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace FlowerApp.Auth.Features.Account.UpdateProfile
{
    public static class UpdateProfileEndpoint
    {
        public static void MapUpdateProfileEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("/users/me/profile", async (
                    [FromForm] UpdateProfileRequest body,
                    ISender sender) =>
            {
                var command = new UpdateProfileCommand(
                    body.FirstName,
                    body.LastName,
                    body.PhoneNumber,
                    body.Gender,
                    body.ProfilePhoto?.OpenReadStream(),
                    body.ProfilePhoto?.FileName);

                var result = await sender.Send(command);

                return result.IsSuccess
                    ? Results.Ok(ApiResponse<UpdateProfileResponse>.Success(result.Value!, "Profile updated successfully."))
                    : result.ToProblemResult();
            })
            .RequireAuthorization()
            .DisableAntiforgery()
            .WithName("UpdateProfile")
            .WithTags("Account")
            .Produces<ApiResponse<UpdateProfileResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
        }
    }
}