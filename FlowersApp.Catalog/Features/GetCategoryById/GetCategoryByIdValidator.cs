using FluentValidation;

namespace FlowersApp.Catalog.Features.GetCategoryById
{
    public class GetCategoryByIdValidator : AbstractValidator<GetCategoryByIdQuery>
    {
        public GetCategoryByIdValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category id is required.");
        }
    }
}
