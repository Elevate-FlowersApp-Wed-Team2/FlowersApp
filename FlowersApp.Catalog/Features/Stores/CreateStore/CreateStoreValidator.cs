using FluentValidation;

namespace FlowersApp.Catalog.Features.Stores.CreateStore
{
    public class CreateStoreValidator : AbstractValidator<CreateStoreCommand>
    {
        public CreateStoreValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        }
    }
}
