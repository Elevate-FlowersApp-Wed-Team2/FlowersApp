using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Catalog.Features.GetCategoryById
{
    public class GetCategoryByIdHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDetailsResponse>
    {
        private readonly Repository<Domain.Entities.Category> _categories;

        public GetCategoryByIdHandler(Repository<Domain.Entities.Category> categories)
        {
            _categories = categories;
        }

        public async Task<RequestResult<CategoryDetailsResponse>> Handle(
            GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            // "never existed" from "exists but archived"
            var category = await _categories
                .Get(c => c.Id == request.CategoryId)
                .FirstOrDefaultAsync(cancellationToken);

            if (category is null)
                return RequestResult<CategoryDetailsResponse>.Failure(ResultCode.CategoryNotFound);

            if (!category.IsActive)
                return RequestResult<CategoryDetailsResponse>.Failure(ResultCode.CategoryArchived);

            var response = new CategoryDetailsResponse(category.Id, category.Name, category.IconUrl);
            return RequestResult<CategoryDetailsResponse>.succeeded(response, ResultCode.CategoryRetrieved);
        }
    }
}
