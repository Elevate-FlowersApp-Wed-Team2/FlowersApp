namespace FlowersApp.Catalog.Features.GetProductById
{
    public record ProductDetailsResponse(
    Guid Id,
    string Name,
    string Description,
    List<string> ImageUrls,
    List<string> Includes,
    decimal OriginalPrice,
    decimal? DiscountedPrice,
    int? DiscountPercentage,
    bool IsOutOfStock,
    int StockQuantity
);
}
