using FluentValidation;

namespace FlowersApp.Auth.Features.DriverApplications.ApplyDriver;

public class ApplyDriverValidator : AbstractValidator<ApplyDriverOrchestrator>
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

    public ApplyDriverValidator()
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
        RuleFor(x => x.LicenceImage)
            .Must(BeAValidFile).WithMessage("Licence image is invalid, too large, or an unsupported format.");

        RuleFor(x => x.NidImage)
            .Must(BeAValidFile).WithMessage("National ID image is invalid, too large, or an unsupported format.");
    }

    private bool BeAValidFile(IFormFile file)
    {
        if (file is null || file.Length == 0 || file.Length > MaxFileSizeBytes)
            return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return false;

        return HasValidMagicBytes(file, extension);
    }

    private static bool HasValidMagicBytes(IFormFile file, string extension)
    {
        using var stream = file.OpenReadStream();
        var buffer = new byte[8];
        var read = stream.Read(buffer, 0, buffer.Length);
        stream.Position = 0;
        if (read < 4) return false;

        return extension switch
        {
            ".jpg" or ".jpeg" => buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF,
            ".png" => buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47,
            ".pdf" => buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46,
            _ => false
        };
    }
}