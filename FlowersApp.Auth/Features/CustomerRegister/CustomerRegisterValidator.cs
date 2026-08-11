using FlowersApp.Auth.Shared.Response;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Auth.Features.CustomerRegister;

public class CustomerRegisterValidator : AbstractValidator<CustomerRegisterCommand>
{
    public CustomerRegisterValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.NameRequired)]);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.EmailRequired)])
            .EmailAddress().WithMessage(localizer[nameof(ResultCode.InvalidEmail)]);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.PhoneRequired)])
            .Matches("^01(?:0|1|2|5)\\d{8}$").WithMessage(localizer[nameof(ResultCode.InvalidPhone)]);

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage(localizer[nameof(ResultCode.InvalidGender)]);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.PasswordRequired)])
            .MinimumLength(6).WithMessage(localizer[nameof(ResultCode.PasswordTooShort)])
            .Matches("[A-Z]").WithMessage(localizer[nameof(ResultCode.PasswordMissingUppercase)])
            .Matches("[0-9]").WithMessage(localizer[nameof(ResultCode.PasswordMissingDigit)]);

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage(localizer[nameof(ResultCode.PasswordMismatch)]);
    }
}
