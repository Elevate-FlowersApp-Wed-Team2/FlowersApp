using FluentValidation;

namespace FlowersApp.Catalog.Features.Stores.GetCoverageGaps
{
    public class GetCoverageGapsValidator : AbstractValidator<GetCoverageGapsQuery>
    {
        public GetCoverageGapsValidator()
        {
            RuleFor(x => x.CheckPoints)
                .NotNull()
                .Must(p => p.Count > 0).WithMessage("At least one point must be provided to check for coverage gaps.")
                .Must(p => p.Count <= 500).WithMessage("A maximum of 500 points can be checked per request.");

            RuleForEach(x => x.CheckPoints).ChildRules(point =>
            {
                point.RuleFor(p => p.Latitude).InclusiveBetween(-90, 90);
                point.RuleFor(p => p.Longitude).InclusiveBetween(-180, 180);
            });
        }
    }
}
