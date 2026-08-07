using FlowerApp.Auth.Domain;
using FlowerApp.Auth.Domain.Enums;
using FlowerApp.Auth.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace FlowerApp.Auth.Features.Account.UpdateProfile
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, Result<UpdateProfileResponse>>
    {
        private const string ProfilePhotoFolder = "profile-photos";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPhotoStorageService _photoStorage;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<UpdateProfileHandler> _logger;

        public UpdateProfileHandler(
            UserManager<ApplicationUser> userManager,
            IPhotoStorageService photoStorage,
            ICurrentUserService currentUser,
            ILogger<UpdateProfileHandler> logger)
        {
            _userManager = userManager;
            _photoStorage = photoStorage;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<Result<UpdateProfileResponse>> Handle(
            UpdateProfileCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUser.UserId is not { } userId)
                return Result<UpdateProfileResponse>.Failure("User is not authenticated.", ErrorType.Unauthorized);

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return Result<UpdateProfileResponse>.Failure("User not found.", ErrorType.NotFound);

            var phoneChanged = !string.Equals(user.PhoneNumber, request.PhoneNumber, StringComparison.Ordinal);

            if (phoneChanged)
            {
                var phoneTaken = await _userManager.Users
                    .AnyAsync(u => u.PhoneNumber == request.PhoneNumber && u.Id != user.Id, cancellationToken);

                if (phoneTaken)
                    return Result<UpdateProfileResponse>.Failure(
                        "Phone number is already in use by another account.", ErrorType.Conflict);
            }

            var photoUrl = user.ProfilePhotoUrl;
            var previousPhotoUrl = user.ProfilePhotoUrl;

            if (request.ProfilePhotoStream is not null && !string.IsNullOrEmpty(request.ProfilePhotoFileName))
            {
                try
                {
                    photoUrl = await _photoStorage.UploadAsync(
                        request.ProfilePhotoStream, request.ProfilePhotoFileName, ProfilePhotoFolder, cancellationToken);
                }
                catch (InvalidOperationException ex)
                {
                    // validation-type failure from the storage service (bad extension/size)
                    return Result<UpdateProfileResponse>.Failure(ex.Message, ErrorType.Validation);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Profile photo upload failed for user {UserId}", user.Id);
                    return Result<UpdateProfileResponse>.Failure(
                        "Failed to upload profile photo. Please try again.", ErrorType.Failure);
                }
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.Gender = request.Gender;
            user.ProfilePhotoUrl = photoUrl;
            user.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                _logger.LogWarning("Profile update failed for user {UserId}: {Errors}", user.Id, errors);
                return Result<UpdateProfileResponse>.Failure(errors, ErrorType.Validation);
            }

            // Best-effort cleanup of the old photo, only after the new one is confirmed saved
            if (!string.IsNullOrEmpty(previousPhotoUrl) && previousPhotoUrl != photoUrl)
            {
                await _photoStorage.DeleteAsync(previousPhotoUrl, cancellationToken);
            }

            return Result<UpdateProfileResponse>.Success(new UpdateProfileResponse(
                user.FirstName,
                user.LastName,
                user.PhoneNumber!,
                user.Gender ?? Gender.Male,
                user.ProfilePhotoUrl));
        }
    }
}

   
