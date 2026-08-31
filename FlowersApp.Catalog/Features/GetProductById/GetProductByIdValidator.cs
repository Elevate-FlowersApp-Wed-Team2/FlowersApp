using FluentValidation;

namespace FlowersApp.Catalog.Features.GetProductById
{
    public class GetProductByIdValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product id is required.");
        }
    }
}
