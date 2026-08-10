using FlowersApp.Auth.Shared.Response;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Auth.Features.Customer.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator(IStringLocalizer<ErrorMessages> localizer)
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage(localizer[nameof(ResultCode.PasswordRequired)]);

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(localizer[nameof(ResultCode.PasswordRequired)])
                .MinimumLength(6).WithMessage(localizer[nameof(ResultCode.PasswordTooShort)])
                .Matches("[A-Z]").WithMessage(localizer[nameof(ResultCode.PasswordMissingUppercase)])
                .Matches("[0-9]").WithMessage(localizer[nameof(ResultCode.PasswordMissingDigit)])
                .NotEqual(x => x.CurrentPassword)
                    .WithMessage(localizer[nameof(ResultCode.NewPasswordSameAsCurrent)]);

            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword)
                    .WithMessage(localizer[nameof(ResultCode.PasswordMismatch)]);
        }
    }
}
