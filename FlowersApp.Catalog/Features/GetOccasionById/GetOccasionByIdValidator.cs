using FluentValidation;

namespace FlowersApp.Catalog.Features.GetOccasionById
{
    public class GetOccasionByIdValidator : AbstractValidator<GetOccasionByIdQuery>
    {
        public GetOccasionByIdValidator()
        {
            RuleFor(x => x.OccasionId)
                .NotEmpty().WithMessage("Occasion id is required.");
        }
    }
}
