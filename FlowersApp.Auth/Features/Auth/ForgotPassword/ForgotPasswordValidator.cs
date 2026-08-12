using FlowersApp.Auth.Shared.Response;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Auth.Features.Auth.ForgotPassword;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.EmailRequired)])
            .EmailAddress().WithMessage(localizer[nameof(ResultCode.InvalidEmail)]);
    }
}
