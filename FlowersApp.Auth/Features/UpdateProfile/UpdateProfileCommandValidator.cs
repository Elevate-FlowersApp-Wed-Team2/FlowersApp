using FlowersApp.Auth.Shared.Response;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Auth.Features.UpdateProfile
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        private static readonly string[] AllowedPhotoContentTypes = { "image/jpeg", "image/png", "image/webp" };

        public UpdateProfileCommandValidator(IStringLocalizer<ErrorMessages> localizer)
        {
            RuleFor(x => x.FullName)
                .MaximumLength(100)
                .Matches(@"^[a-zA-Z\u0600-\u06FF\s'-]+$")
                    .WithMessage(localizer[nameof(ResultCode.NameChractersMismatch)])
                .When(x => !string.IsNullOrWhiteSpace(x.FullName));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9]{7,15}$")
                    .WithMessage(localizer[nameof(ResultCode.InvalidPhone)])
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

            RuleFor(x => x.Gender)
                .IsInEnum()
                    .WithMessage(localizer[nameof(ResultCode.InvalidGender)])
                .When(x => x.Gender.HasValue);

            RuleFor(x => x.ProfilePhotoContentType)
                .Must(ct => AllowedPhotoContentTypes.Contains(ct))
                    .WithMessage(localizer[nameof(ResultCode.InvalidProfilePhoto)])
                .When(x => x.ProfilePhotoStream is not null);

            RuleFor(x => x)
                .Must(x => x.FullName is not null
                    || x.Gender is not null
                    || x.PhoneNumber is not null
                    || x.ProfilePhotoStream is not null)
                    .WithMessage(localizer[nameof(ResultCode.NothingToUpdate)])
                    .WithName("Profile");
        }
    }
}
