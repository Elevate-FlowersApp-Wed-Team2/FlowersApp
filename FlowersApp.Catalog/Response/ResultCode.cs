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
    OccasionsRetrieved = 200,
    OccasionRetrieved = 201,
    OccasionNotFound = 202,
    OccasionArchived = 203,

    // Sections 
    SectionRetrieved = 200,
    NotSupportedLanguage = 201,
    FailedToSaveSection =202,
    SectionSavedSuccesfully =203,
    SectionsNotFound = 204,
    CanNotUpdateSections = 205,
    SectionsUpdatedSuccesfully = 206,

    //Product
    ProductNotFound = 300,
    ProductRetrieved = 301
}
