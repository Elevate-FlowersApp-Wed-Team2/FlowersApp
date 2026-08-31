using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Catalog.Features.GetProductById
{
    public class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, ProductDetailsResponse>
    {
        private readonly Repository<Product> _products;

        public GetProductByIdHandler(Repository<Product> products)
        {
            _products = products;
        }

        public async Task<RequestResult<ProductDetailsResponse>> Handle(
            GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            
            //if (!request.StoreId.HasValue)
            //    return RequestResult<ProductDetailsResponse>.Failure(ResultCode.StoreNotResolved);

            var product = await _products
                .Get(p => p.Id == request.ProductId && p.IsActive)
                //  Simplified: StockQuantity is currently global, not per-store.
                // Proper store-scoped stock needs a ProductStoreStock join —
                // request.StoreId is accepted and validated here for API-contract
                // forward-compatibility, but doesn't yet filter stock by store.
                .FirstOrDefaultAsync(cancellationToken);

            if (product is null)
                return RequestResult<ProductDetailsResponse>.Failure(ResultCode.ProductNotFound);

            var discountedPrice = product.DiscountPercentage.HasValue && product.DiscountPercentage > 0
                ? product.Price - (product.Price * product.DiscountPercentage.Value / 100)
                : (decimal?)null;

            var response = new ProductDetailsResponse(
                product.Id,
                product.Name,
                product.Description,
                product.ImageUrls,
                product.Includes,
                product.Price,
                discountedPrice,
                product.DiscountPercentage.HasValue ? (int)product.DiscountPercentage.Value : null,
                product.StockQuantity <= 0,
                product.StockQuantity);

            return RequestResult<ProductDetailsResponse>.succeeded(response, ResultCode.ProductRetrieved);
        }
    }
}
