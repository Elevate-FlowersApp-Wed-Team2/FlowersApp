using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Catalog.Features.GetCategories
{
    public class GetCategoriesHandler : IQueryHandler<GetCategoriesQuery, List<CategoryResponse>>
    {
        private readonly Repository<Domain.Entities.Category> _categories;

        public GetCategoriesHandler(Repository<Category> categories)
        {
            _categories = categories;
        }

        public async Task<RequestResult<List<CategoryResponse>>> Handle(
            GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var result = await _categories
                .Get(c => c.IsActive)
                .OrderBy(c => c.SortOrder)
                .Select(c => new CategoryResponse(c.Id, c.Name, c.IconUrl))
                .ToListAsync(cancellationToken);

            // this is a live DB query on every request, so Admin changes
            // (add/rename/archive) are reflected immediately with no app update needed.
            return RequestResult<List<CategoryResponse>>.succeeded(result, ResultCode.CategoriesRetrieved);
        }
    }
}
