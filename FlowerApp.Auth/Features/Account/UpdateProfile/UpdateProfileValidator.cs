using FluentValidation;

namespace FlowerApp.Auth.Features.Account.UpdateProfile
{
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
    {
        private static readonly string[] AllowedPhotoExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const int MaxPhotoSizeBytes = 5 * 1024 * 1024; // 5MB

        public UpdateProfileValidator()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50)
            .Matches(@"^[a-zA-Z\u0600-\u06FF\s]+$").WithMessage("First name contains invalid characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50)
                .Matches(@"^[a-zA-Z\u0600-\u06FF\s]+$").WithMessage("Last name contains invalid characters.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Gender value is invalid.");

            RuleFor(x => x.ProfilePhotoFileName)
                .Must(name => name is null || AllowedPhotoExtensions.Contains(Path.GetExtension(name).ToLowerInvariant()))
                .WithMessage("Photo must be JPG, PNG, or WEBP.");
        }

    }
}
