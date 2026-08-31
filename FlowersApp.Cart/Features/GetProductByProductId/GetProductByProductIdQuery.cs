using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;

namespace FlowersApp.Cart.Features.GetProductByProductId;

public record GetProductByProductIdQuery
(string ProductId) : IQuery<CatalogProductResponse>;

public class GetProductByProductIdQueryHandler(ICatalogApiClient catalogApiClient)
    : IQueryHandler<GetProductByProductIdQuery, CatalogProductResponse>
{
    private readonly ICatalogApiClient _catalogApiClient = catalogApiClient;

    public async Task<RequestResult<CatalogProductResponse>> Handle(
        GetProductByProductIdQuery request, CancellationToken cancellationToken)
    {
        if(!Guid.TryParse(request.ProductId, out var productId))
            return RequestResult<CatalogProductResponse>.Failure(ResultCode.ProductNotFound);
        var product = await _catalogApiClient.GetProductAsync(
            productId, cancellationToken);

        if (product is null)
            return RequestResult<CatalogProductResponse>.Failure(ResultCode.ProductNotFound);

        return RequestResult<CatalogProductResponse>.succeeded(product, ResultCode.ProductAddedSuccesfully);
    }
}
