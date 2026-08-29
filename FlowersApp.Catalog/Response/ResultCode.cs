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

    // Occasion
    OccasionsRetrieved = 300,
    OccasionRetrieved = 301,
    OccasionNotFound = 302,
    OccasionArchived = 303,

    // Sections 
    SectionRetrieved = 200,
    NotSupportedLanguage = 201,
    FailedToSaveSection =202,
    SectionSavedSuccesfully =203,
    SectionsNotFound = 204,
    CanNotUpdateSections = 205,
    SectionsUpdatedSuccesfully = 206,

    // Product
    ProductRetrieved = 400,
    ProductNotFound = 401,
    StoreNotResolved = 402,
}
