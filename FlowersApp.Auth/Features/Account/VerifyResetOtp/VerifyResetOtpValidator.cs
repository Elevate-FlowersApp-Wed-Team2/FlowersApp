using FluentValidation;

namespace FlowersApp.Auth.Features.Account.VerifyResetOtp
{
    public class VerifyResetOtpValidator : AbstractValidator<VerifyResetOtpCommand>
    {
        public VerifyResetOtpValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email format is invalid.");

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("Verification code is required.")
                .Matches(@"^\d{6}$").WithMessage("Verification code must be 6 digits.");
        }
    }
}
