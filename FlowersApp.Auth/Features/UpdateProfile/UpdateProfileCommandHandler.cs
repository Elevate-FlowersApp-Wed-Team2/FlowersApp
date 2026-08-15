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
        private readonly ILogger<UpdateProfileCommandHandler> _logger;
        public UpdateProfileCommandHandler(
            UserManager<AppUser> userManager,
            ICurrentUserService currentUserService,
            IPhotoStorageService photoStorage,
            ILogger<UpdateProfileCommandHandler> logger)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _photoStorage = photoStorage;
            _logger = logger;
        }
        public async Task<RequestResult<UpdateProfileResponse>> Handle(
            UpdateProfileCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            _logger.LogInformation("Attempting profile update for user {UserId}.", userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                _logger.LogWarning("Profile update failed: User {UserId} not found.", userId);
                return RequestResult<UpdateProfileResponse>.Failure(ResultCode.UserNotFound);
            }
            // --- Phone number update ---
            if (request.PhoneNumber is not null && request.PhoneNumber != user.PhoneNumber)
            {
                var phoneTaken = await _userManager.Users
                    .AnyAsync(u => u.PhoneNumber == request.PhoneNumber
                                && u.Id != user.Id, cancellationToken);
                if (phoneTaken)
                {
                    _logger.LogWarning("Profile update failed for user {UserId}: Phone number {PhoneNumber} already in use.", userId, request.PhoneNumber);
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
            // --- Photo upload ---
            // Capture old URL before overwriting — used for cleanup after successful DB save
            string? newPhotoUrl = null;
            var oldPhotoUrl = user.ProfilePhotoUrl;
            if (request.Photo is not null)
            {
                try
                {
                    newPhotoUrl = await _photoStorage.UploadAsync(
                        request.Photo.Stream,
                        request.Photo.FileName,
                        request.Photo.ContentType,
                        cancellationToken);
                    user.ProfilePhotoUrl = newPhotoUrl;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Photo upload failed for user {UserId}", userId);
                    return RequestResult<UpdateProfileResponse>.Failure(ResultCode.PhotoUploadFailed);
                }
            }
            //save to DB
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                if (newPhotoUrl is not null)
                {
                    await _photoStorage.DeleteAsync(newPhotoUrl, cancellationToken);
                }
                _logger.LogError(
                    "Profile update failed for user {UserId}. Errors: {Errors}",
                    userId,
                    string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                return RequestResult<UpdateProfileResponse>.Failure(ResultCode.ProfileUpdateFailed);
            }
            // Cleanup: delete the old photo from storage now that DB is committed
            if (newPhotoUrl is not null && oldPhotoUrl is not null)
            {
                await _photoStorage.DeleteAsync(oldPhotoUrl, cancellationToken);
            }
            var response = new UpdateProfileResponse
            {
                FullName = user.FullName,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                ProfilePhotoUrl = user.ProfilePhotoUrl
            };
            _logger.LogInformation("Profile updated successfully for user {UserId}.", userId);
            return RequestResult<UpdateProfileResponse>.succeeded(response, ResultCode.ProfileUpdatedSuccessfully);
        }
    }
}
