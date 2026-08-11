using FlowersApp.Auth.Shared.Response;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Auth.Features.UpdateProfile
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        private static readonly string[] AllowedPhotoContentTypes =
        {
            "image/jpeg",
            "image/png",
            "image/webp"
        };
        public UpdateProfileCommandValidator(IStringLocalizer<ErrorMessages> localizer)
        {
            // --- Full Name ---
            RuleFor(x => x.FullName)
                .MaximumLength(100)
                .Matches(@"^[a-zA-Z\u0600-\u06FF\s'-]+$")
                    .WithMessage(localizer[nameof(ResultCode.NameCharactersMismatch)])
                .When(x => !string.IsNullOrWhiteSpace(x.FullName));
            // --- Phone Number ---
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9]{7,15}$")
                    .WithMessage(localizer[nameof(ResultCode.InvalidPhone)])
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
            RuleFor(x => x.Gender)
                .IsInEnum()
                    .WithMessage(localizer[nameof(ResultCode.InvalidGender)])
                .When(x => x.Gender.HasValue);
            // --- Profile Photo: validate ContentType only when a photo is provided ---
            RuleFor(x => x.Photo!.ContentType)
                .Must(ct => AllowedPhotoContentTypes.Contains(ct))
                    .WithMessage(localizer[nameof(ResultCode.InvalidProfilePhoto)])
                .When(x => x.Photo is not null);
            // --- At least one field must be provided ---
            RuleFor(x => x)
                .Must(x => x.FullName is not null
                        || x.Gender is not null
                        || x.PhoneNumber is not null
                        || x.Photo is not null)
                    .WithMessage(localizer[nameof(ResultCode.NothingToUpdate)])
                    .WithName("Profile");
        }
    }
}
