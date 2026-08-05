using FluentValidation;

namespace FlowersApp.Auth.Features.Driver.SubmitApplication;

public class SubmitApplicationCommandValidator : AbstractValidator<SubmitApplicationCommand>
{
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
    private const int MaxFileSize = 5 * 1024 * 1024; // 5MB

    public SubmitApplicationCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format");

        RuleFor(x => x.NationalIdNumber)
            .NotEmpty().WithMessage("National ID number is required")
            .MaximumLength(20).WithMessage("National ID cannot exceed 20 characters");

        RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("License number is required")
            .MaximumLength(50).WithMessage("License number cannot exceed 50 characters");

        RuleFor(x => x.VehicleType)
            .NotEmpty().WithMessage("Vehicle type is required")
            .Must(type => type == "motorcycle" || type == "car")
            .WithMessage("Vehicle type must be either 'motorcycle' or 'car'");

        RuleFor(x => x.VehiclePlateNumber)
            .NotEmpty().WithMessage("Vehicle plate number is required")
            .MaximumLength(20).WithMessage("Vehicle plate number cannot exceed 20 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .Must(password => password.Any(char.IsUpper)).WithMessage("Password must contain at least one uppercase letter")
            .Must(password => password.Any(char.IsDigit)).WithMessage("Password must contain at least one digit");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match");

        // File validation
        RuleFor(x => x.Documents)
            .NotEmpty().WithMessage("At least one document must be uploaded")
            .Must(docs => docs.Count <= 5).WithMessage("Maximum 5 documents allowed")
            .Must(docs => docs.All(d => IsValidFileExtension(d.FileName)))
            .WithMessage($"Only {string.Join(", ", _allowedExtensions)} files are allowed")
            .Must(docs => docs.All(d => d.Length <= MaxFileSize))
            .WithMessage($"Each file must be {MaxFileSize / (1024 * 1024)}MB or less");
    }

    private bool IsValidFileExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return _allowedExtensions.Contains(extension);
    }
}