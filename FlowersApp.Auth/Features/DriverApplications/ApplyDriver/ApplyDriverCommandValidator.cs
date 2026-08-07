using FluentValidation;

namespace FlowersApp.Auth.Features.DriverApplications.SubmitApplication;

public class ApplyDriverCommandValidator : AbstractValidator<ApplyDriverCommand>
{
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
    private const int MaxFileSize = 5 * 1024 * 1024; // 5MB

    public ApplyDriverCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format");

        RuleFor(x => x.Nid)
            .NotEmpty().WithMessage("National ID number is required")
            .MaximumLength(20).WithMessage("National ID cannot exceed 20 characters");

        RuleFor(x => x.LicenceNumber)
            .NotEmpty().WithMessage("License number is required")
            .MaximumLength(50).WithMessage("License number cannot exceed 50 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .Must(password => password.Any(char.IsUpper)).WithMessage("Password must contain at least one uppercase letter")
            .Must(password => password.Any(char.IsDigit)).WithMessage("Password must contain at least one digit");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match");

    }

    private bool IsValidFileExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return _allowedExtensions.Contains(extension);
    }
}