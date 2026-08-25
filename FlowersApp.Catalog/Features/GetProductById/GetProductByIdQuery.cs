using FlowersApp.Catalog.Shared.Interfaces;

namespace FlowersApp.Catalog.Features.GetProductById
{
    public record GetProductByIdQuery(Guid ProductId, Guid? StoreId)
        : IQuery<ProductDetailsResponse>;
}
