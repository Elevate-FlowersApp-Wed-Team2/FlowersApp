using FlowersApp.Auth.Shared.Response;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Auth.Features.Auth.ResetPassword;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.ResetToken)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.InvalidOrExpiredResetToken)]);

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.PasswordRequired)])
            .MinimumLength(6).WithMessage(localizer[nameof(ResultCode.PasswordTooShort)])
            .Matches("[A-Z]").WithMessage(localizer[nameof(ResultCode.PasswordMissingUppercase)])
            .Matches("[0-9]").WithMessage(localizer[nameof(ResultCode.PasswordMissingDigit)]);

        RuleFor(x => x.ConfirmNewPassword)
            .Equal(x => x.NewPassword)
            .WithMessage(localizer[nameof(ResultCode.PasswordMismatch)]);
    }
}
