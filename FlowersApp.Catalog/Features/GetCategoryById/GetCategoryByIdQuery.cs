using FlowersApp.Catalog.Shared.Interfaces;

namespace FlowersApp.Catalog.Features.GetCategoryById
{
    public record GetCategoryByIdQuery(Guid CategoryId) 
        : IQuery<CategoryDetailsResponse>;
}
