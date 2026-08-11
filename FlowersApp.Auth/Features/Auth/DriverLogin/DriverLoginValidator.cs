using FlowersApp.Auth.Shared.Response;
using FluentValidation;

namespace FlowersApp.Auth.Features.Auth.DriverLogin;

public class DriverLoginValidator : AbstractValidator<DriverLoginOrchestrator>
{
    public DriverLoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode(ResultCode.EmailRequired.ToString())
            .EmailAddress().WithErrorCode(ResultCode.InvalidEmail.ToString());

        RuleFor(x => x.Password)
            .NotEmpty().WithErrorCode(ResultCode.PasswordRequired.ToString());
    }
}
