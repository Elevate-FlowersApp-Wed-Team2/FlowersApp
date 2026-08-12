using FlowersApp.Auth.Shared.Response;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Auth.Features.Auth.VerifyOtp;

public class VerifyOtpValidator : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.EmailRequired)])
            .EmailAddress().WithMessage(localizer[nameof(ResultCode.InvalidEmail)]);

        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.OtpRequired)])
            .Length(6).WithMessage(localizer[nameof(ResultCode.InvalidOrExpiredOtp)])
            .Matches("^[0-9]{6}$").WithMessage(localizer[nameof(ResultCode.InvalidOrExpiredOtp)]);
    }
}
