namespace FlowersApp.Catalog.Shared.Constants;

public static class Endpoints
{
    private const string BasePath = "api/v1";
    public static class Catalog
    {
        private const string CatalogBase = $"{BasePath}/catalog";

        public const string GetProducts = $"{CatalogBase}/products";
    }
    public static class Category
    {
        private const string CategoryBase = $"{BasePath}/catalog/categories";
        public const string GetCategories = CategoryBase;
        public const string GetCategoryById = $"{CategoryBase}/{{id}}";

    }

    public static class Occasion
    {
        private const string OccasionBase = $"{BasePath}/catalog/occasions";

        public const string GetOccasions = OccasionBase;
        public const string GetOccasionById = $"{OccasionBase}/{{id}}";
    }
}
