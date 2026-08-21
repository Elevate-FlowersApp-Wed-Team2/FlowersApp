namespace FlowersApp.Catalog.Domain.Enum;

public enum SectionType
{
    ProductRail = 1,
    CategoryRail,
    OccasionRail,
    Banner,
    BestSeller,
}

public static class HomeSectionTypeExtensions
{
    /// <summary>The wire value used in the JSON "type" discriminator, e.g. "product_rail".</summary>
    public static string ToWireValue(this SectionType type) => type switch
    {
        SectionType.Banner => "banner",
        SectionType.CategoryRail => "category_rail",
        SectionType.ProductRail => "product_rail",
        SectionType.OccasionRail => "occasion_rail",
        SectionType.BestSeller => "best_seller",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    public static bool TryParseWireValue(string? value, out SectionType type)
    {
        switch (value)
        {
            case "banner": type = SectionType.Banner; return true;
            case "category_rail": type = SectionType.CategoryRail; return true;
            case "product_rail": type = SectionType.ProductRail; return true;
            case "occasion_rail": type = SectionType.OccasionRail; return true;
            case "best_seller": type= SectionType.BestSeller; return true;
            default: type = default; return false;
        }
    }
}