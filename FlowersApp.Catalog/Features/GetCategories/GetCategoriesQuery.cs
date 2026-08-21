using FlowersApp.Catalog.Shared.Interfaces;

namespace FlowersApp.Catalog.Features.GetCategories
{
    public record GetCategoriesQuery : IQuery<List<CategoryResponse>>;
}
