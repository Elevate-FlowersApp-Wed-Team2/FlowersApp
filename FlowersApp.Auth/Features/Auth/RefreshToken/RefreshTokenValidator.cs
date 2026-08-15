using FlowersApp.Auth.Shared.Response;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Auth.Features.Auth.RefreshToken;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage(localizer[nameof(ResultCode.InvalidRefreshToken)]);
    }
}
