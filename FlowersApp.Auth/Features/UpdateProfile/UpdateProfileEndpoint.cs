using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.UpdateProfile
{
    public class UpdateProfileEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut(Endpoints.Users.UpdateProfile, async (
                [FromForm] UpdateProfileDTO request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateProfileCommand
                {
                    FullName = request.FullName,
                    Gender = request.Gender,
                    PhoneNumber = request.PhoneNumber,
                    ProfilePhotoStream = request.ProfilePhoto?.OpenReadStream(),
                    ProfilePhotoFileName = request.ProfilePhoto?.FileName,
                    ProfilePhotoContentType = request.ProfilePhoto?.ContentType
                };

                var result = await mediator.Send(command, cancellationToken);

                return result.Code switch
                {
                    ResultCode.ProfileUpdatedSuccessfully => Results.Ok(
                        ApiResponse<UpdateProfileResponse>.Success(result.Result, HttpStatusCode.OK, result.Message)),

                    ResultCode.UserNotFound => Results.NotFound(
                        ApiResponse<UpdateProfileResponse>.Failure(result.Message ?? string.Empty, HttpStatusCode.NotFound)),

                    ResultCode.PhoneAlreadyInUse => Results.Conflict(
                        ApiResponse<UpdateProfileResponse>.Failure(result.Message ?? string.Empty, HttpStatusCode.Conflict)),

                    
                    _ => Results.BadRequest(
                        ApiResponse<UpdateProfileResponse>.Failure(result.Message ?? string.Empty))
                };
            })
            .Accepts<UpdateProfileDTO>("multipart/form-data")
            .DisableAntiforgery()
            .RequireAuthorization()
            .Produces<ApiResponse<UpdateProfileResponse>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<UpdateProfileResponse>>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse<UpdateProfileResponse>>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse<UpdateProfileResponse>>(StatusCodes.Status409Conflict)
            .WithName("UpdateProfile")
            .WithTags("Users");
        }
    }
}
