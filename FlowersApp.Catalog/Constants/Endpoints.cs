namespace FlowersApp.Catalog.Shared.Constants;

public static class Endpoints
{
    private const string BasePath = "api/v1";
    public static class Catalog
    {
        private const string CatalogBase = $"{BasePath}/catalog";

        public const string GetProducts = $"{CatalogBase}/products";
    }
}
