using System.Text.Json.Serialization;

namespace FloweryApp.Api.Features.Home.GetHomeLayout;

// ---- shared item shapes (kept lightweight — same "summary" shape used across rails) ----

public sealed record CategorySummaryDto(int Id, string Name, string IconUrl);

public sealed record OccasionSummaryDto(int Id, string Name, string ImageUrl);

public sealed record ProductSummaryDto(
    int Id,
    string Name,
    string ImageUrl,
    string Currency,
    decimal Price,
    decimal? OriginalPrice,
    decimal? DiscountPercentage,
    string Status);

/// <summary>
/// Base shape every Home section carries regardless of type (AC1: {type, id, title?, order,
/// enabled} + a type-specific payload). System.Text.Json's built-in polymorphism serializes the
/// concrete derived record's extra properties alongside these, discriminated by "type" — the
/// exact field the client is told to switch on (AC2/AC3).
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type", UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
[JsonDerivedType(typeof(BannerSectionDto), "banner")]
[JsonDerivedType(typeof(CategoryRailSectionDto), "category_rail")]
[JsonDerivedType(typeof(ProductRailSectionDto), "product_rail")]
[JsonDerivedType(typeof(OccasionRailSectionDto), "occasion_rail")]
public abstract record HomeLayoutSectionDto(int Id, string? Title, int Order, bool Enabled);

public sealed record BannerSectionDto(
    int Id,
    string? Title,
    int Order,
    bool Enabled,
    string ImageUrl,
    string DeepLink)
    : HomeLayoutSectionDto(Id, Title, Order, Enabled);

public sealed record CategoryRailSectionDto(
    int Id,
    string? Title,
    int Order,
    bool Enabled,
    string ViewAllDeepLink,
    IReadOnlyList<CategorySummaryDto> Categories)
    : HomeLayoutSectionDto(Id, Title, Order, Enabled);

public sealed record ProductRailSectionDto(
    int Id,
    string? Title,
    int Order,
    bool Enabled,
    string ViewAllDeepLink,
    IReadOnlyList<ProductSummaryDto> Products)
    : HomeLayoutSectionDto(Id, Title, Order, Enabled);

public sealed record OccasionRailSectionDto(
    int Id,
    string? Title,
    int Order,
    bool Enabled,
    string ViewAllDeepLink,
    IReadOnlyList<OccasionSummaryDto> Occasions)
    : HomeLayoutSectionDto(Id, Title, Order, Enabled);
