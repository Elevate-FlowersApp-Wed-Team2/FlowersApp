namespace FlowersApp.Catalog.Features.GetProductCatalog
{
    public record ProductCatalogItemResponse(
     Guid Id,
     string Name,
     string ImageUrl,
     decimal OriginalPrice,
     decimal? DiscountedPrice,
     int? DiscountPercentage,
     bool IsOutOfStock
    );
}
