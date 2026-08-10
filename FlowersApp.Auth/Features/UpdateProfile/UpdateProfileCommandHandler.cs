using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Features.UpdateProfile
{
    public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, UpdateProfileResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPhotoStorageService _photoStorage;

        public UpdateProfileCommandHandler(
            UserManager<AppUser> userManager,
            ICurrentUserService currentUserService,
            IPhotoStorageService photoStorage)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _photoStorage = photoStorage;
        }

        public async Task<RequestResult<UpdateProfileResponse>> Handle(
            UpdateProfileCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                return RequestResult<UpdateProfileResponse>.Failure(ResultCode.UserNotFound);
            }

            
            if (request.PhoneNumber is not null && request.PhoneNumber != user.PhoneNumber)
            {
                var phoneTaken = await _userManager.Users
                    .AnyAsync(u => u.PhoneNumber == request.PhoneNumber && u.Id != user.Id, cancellationToken);

                if (phoneTaken)
                {
                    return RequestResult<UpdateProfileResponse>.Failure(ResultCode.PhoneAlreadyInUse);
                }

                user.PhoneNumber = request.PhoneNumber;
                user.PhoneNumberConfirmed = false;
            }

            if (request.FullName is not null)
            {
                user.FullName = request.FullName;
            }

            if (request.Gender.HasValue)
            {
                user.Gender = request.Gender.Value;
            }

            if (request.ProfilePhotoStream is not null)
            {
                try
                {
                    user.ProfilePhotoUrl = await _photoStorage.UploadAsync(
                        request.ProfilePhotoStream,
                        request.ProfilePhotoFileName ?? $"{user.Id}.jpg",
                        request.ProfilePhotoContentType ?? "image/jpeg",
                        cancellationToken);
                }
                catch (Exception)
                {
                    return RequestResult<UpdateProfileResponse>.Failure(ResultCode.PhotoUploadFailed);
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return RequestResult<UpdateProfileResponse>.Failure(ResultCode.ProfileUpdateFailed);
            }

            var response = new UpdateProfileResponse
            {
                FullName = user.FullName,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                ProfilePhotoUrl = user.ProfilePhotoUrl
            };

            return RequestResult<UpdateProfileResponse>.succeeded(response, ResultCode.ProfileUpdatedSuccessfully);
        }
    }
}
