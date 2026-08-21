using FlowersApp.Catalog.Features.GetProductCatalog;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;

namespace FlowersApp.Catalog.Features.GetProductCatalog
{
    public record GetProductCatalogQuery(
    int PageNumber,
    int PageSize,
    Guid? StoreId,
    Guid? CategoryId
     ) : IQuery<PagedResult<ProductCatalogItemResponse>>;
}
