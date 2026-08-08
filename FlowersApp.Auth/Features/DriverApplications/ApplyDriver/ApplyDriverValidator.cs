using FlowersApp.Auth.Shared.Response;
using FluentValidation;

namespace FlowersApp.Auth.Features.DriverApplications.ApplyDriver;

public class ApplyDriverValidator : AbstractValidator<ApplyDriverOrchestrator>
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

    public ApplyDriverValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithErrorCode(ResultCode.NameRequired.ToString())
            .MaximumLength(100).WithErrorCode(ResultCode.NameChractersMismatch.ToString());

        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode(ResultCode.EmailRequired.ToString())
            .EmailAddress().WithErrorCode(ResultCode.InvalidEmail.ToString())
            .MaximumLength(255).WithErrorCode(ResultCode.EmailTooLong.ToString());

        RuleFor(x => x.Phone)
            .NotEmpty().WithErrorCode(ResultCode.PhoneRequired.ToString())
            .Matches(@"^\+?[1-9]\d{1,14}$").WithErrorCode(ResultCode.InvalidPhone.ToString());

        RuleFor(x => x.Nid)
            .NotEmpty().WithErrorCode(ResultCode.NidRequired.ToString())
            .MaximumLength(20).WithErrorCode(ResultCode.NidTooLong.ToString());

        RuleFor(x => x.LicenceNumber)
            .NotEmpty().WithErrorCode(ResultCode.LicenceNumberRequired.ToString())
            .MaximumLength(50).WithErrorCode(ResultCode.LicenceNumberTooLong.ToString());

        RuleFor(x => x.Password)
            .NotEmpty().WithErrorCode(ResultCode.PasswordRequired.ToString())
            .MinimumLength(6).WithErrorCode(ResultCode.PasswordTooShort.ToString())
            .Must(password => password.Any(char.IsUpper)).WithErrorCode(ResultCode.PasswordMissingUppercase.ToString())
            .Must(password => password.Any(char.IsDigit)).WithErrorCode(ResultCode.PasswordMissingDigit.ToString());

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithErrorCode(ResultCode.PasswordMismatch.ToString());

        RuleFor(x => x.LicenceImage)
            .Must(BeAValidFile).WithErrorCode(ResultCode.LicenceImageInvalid.ToString());

        RuleFor(x => x.NidImage)
            .Must(BeAValidFile).WithErrorCode(ResultCode.NidImageInvalid.ToString());
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