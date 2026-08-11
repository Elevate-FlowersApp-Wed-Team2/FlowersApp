using FlowersApp.Auth.Shared.Response;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Auth.Features.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.EmailRequired)])
            .EmailAddress().WithMessage(localizer[nameof(ResultCode.InvalidEmail)]);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.PasswordRequired)]);
    }
}
