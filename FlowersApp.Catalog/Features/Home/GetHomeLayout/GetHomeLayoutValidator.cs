using FluentValidation;

namespace FloweryApp.Api.Features.Home.GetHomeLayout;

public sealed class GetHomeLayoutValidator : AbstractValidator<GetHomeLayoutQuery>
{
    public GetHomeLayoutValidator()
    {
        RuleFor(x => x.AcceptLanguage)
            .NotEmpty()
            .Must(lang => lang is "ar" or "en")
            .WithMessage("Accept-Language must be 'ar' or 'en'.");

        RuleFor(x => x.StoreId)
            .GreaterThan(0)
            .When(x => x.StoreId.HasValue)
            .WithMessage("storeId must be a positive integer when provided.");
    }
}
