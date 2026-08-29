namespace FlowersApp.Cart.Shared.Interfaces;

public interface ICatalogApiClient
{
    Task<CatalogProductResponse?> GetProductAsync(Guid productId, CancellationToken cancellationToken);
}


public record CatalogProductResponse(Guid Id,
string Name,
string Description,
List<string> ImageUrls,
List<string> Includes,
decimal OriginalPrice,
decimal? DiscountedPrice,
int? DiscountPercentage,
bool IsOutOfStock,
int StockQuantity);