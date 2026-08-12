using FlowersApp.Auth.Shared.Response;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Auth.Features.Auth.DriverLogin;

public class DriverLoginValidator : AbstractValidator<DriverLoginCommand>
{
    public DriverLoginValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.EmailRequired)])
            .EmailAddress().WithMessage(localizer[nameof(ResultCode.InvalidEmail)]);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.PasswordRequired)]);
    }
}
