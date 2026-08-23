namespace FlowersApp.Cart.Shared.Interfaces;

public interface ICatalogApiClient
{
    Task<CatalogProductResponse?> GetProductAsyncGetProductAsync(string productId, CancellationToken cancellationToken);
}

public record CatalogProductResponse(string Id, string Name, decimal Price, bool InStock, int AvailableQty);