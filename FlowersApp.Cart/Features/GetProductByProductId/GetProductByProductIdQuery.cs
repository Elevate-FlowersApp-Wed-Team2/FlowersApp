using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;

namespace FlowersApp.Cart.Features.GetProductByProductId;

public record GetProductByProductIdQuery
(string ProductId) : IQuery<GetProductByProductIdResponse>;

public record GetProductByProductIdResponse(string ProductName, int Stock, decimal Price);

public class GetProductByProductIdQueryHandler(ICatalogApiClient catalogApiClient)
    : IQueryHandler<GetProductByProductIdQuery, GetProductByProductIdResponse>
{
    private readonly ICatalogApiClient _catalogApiClient = catalogApiClient;

    public async Task<RequestResult<GetProductByProductIdResponse>> Handle(
        GetProductByProductIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _catalogApiClient.GetProductAsync(
            request.ProductId, cancellationToken);

        if (product is null)
            return RequestResult<GetProductByProductIdResponse>.Failure(ResultCode.ProductNotFound);

        var response = new GetProductByProductIdResponse(product.Name, product.AvailableQty, product.Price);

        return RequestResult<GetProductByProductIdResponse>.succeeded(response,ResultCode.ProductAddedSuccesfully);
    }
}
