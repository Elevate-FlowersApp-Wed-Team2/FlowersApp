using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.UpdateProfile
{
    file static class PhotoSizeLimit
    {
        internal const long MaxBytes = 5 * 1024 * 1024; // 5 MB
    }
    public class UpdateProfileEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut(Endpoints.Users.UpdateProfile, async (
                [FromForm] UpdateProfileDTO request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                // Guard file size at the endpoint boundary before dispatching
                if (request.ProfilePhoto is not null
                    && request.ProfilePhoto.Length > PhotoSizeLimit.MaxBytes)
                {
                    return Results.BadRequest(
                        ApiResponse<UpdateProfileResponse>.Failure(
                            "Profile photo must be 5 MB or smaller."));
                }
                var command = new UpdateProfileCommand
                {
                    FullName = request.FullName,
                    Gender = request.Gender,
                    PhoneNumber = request.PhoneNumber,
                    Photo = request.ProfilePhoto is null
                        ? null
                        : new PhotoUpload(
                            request.ProfilePhoto.OpenReadStream(),
                            request.ProfilePhoto.FileName,
                            request.ProfilePhoto.ContentType)
                };
                var result = await mediator.Send(command, cancellationToken);
                return result.Code switch
                {
                    ResultCode.ProfileUpdatedSuccessfully => Results.Ok(
                        ApiResponse<UpdateProfileResponse>.Success(
                            result.Result, HttpStatusCode.OK, result.Message)),
                    ResultCode.UserNotFound => Results.NotFound(
                        ApiResponse<UpdateProfileResponse>.Failure(
                            result.Message ?? string.Empty, HttpStatusCode.NotFound)),
                    ResultCode.PhoneAlreadyInUse => Results.Conflict(
                        ApiResponse<UpdateProfileResponse>.Failure(
                            result.Message ?? string.Empty, HttpStatusCode.Conflict)),
                    ResultCode.PhotoUploadFailed => Results.Json(
                        ApiResponse<UpdateProfileResponse>.Failure(result.Message ?? string.Empty),
                        statusCode: StatusCodes.Status500InternalServerError),
                    ResultCode.ProfileUpdateFailed => Results.Json(
                        ApiResponse<UpdateProfileResponse>.Failure(result.Message ?? string.Empty),
                        statusCode: StatusCodes.Status500InternalServerError),
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
            .Produces<ApiResponse<UpdateProfileResponse>>(StatusCodes.Status500InternalServerError)
            .WithName("UpdateProfile")
            .WithTags("Users");
        }
    }
}
