namespace FlowersApp.Catalog.Shared.Response;

public enum ResultCode
{
    // Catalog (1-99)
    CatalogRetrieved = 1,

    // Category
    CategoriesRetrieved = 100,
    CategoryRetrieved = 101,
    CategoryNotFound = 102,
    CategoryArchived = 103,
}
