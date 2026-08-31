namespace FlowersApp.Cart.Shared.Response;

public enum ResultCode
{
    //Cart
    FailedToInitiateCart = 100,
    CartInitiatedSuccesfully = 101,
    NotFoundCartForThisUser = 102,
    CartRetrivedSuccesfully = 103,
    ProductNotFound = 104,
    ProductAddedSuccesfully = 105,
    NoValidQuantity = 106,
    ProductIsOutOfStock = 107,
    InvalidQuantity = 108,
    InvalidProductId = 109,
    CartRetrievalFailed = 110,
    CartInitiatedSuccessfully = 111,
    //ShoopingCartItem
    CartItemNotFound = 201,
    CartItemUpdatedSuccesfully = 202,
    CanNotUpdateCartItem = 203,
    CartItemCreatedSuccesfully = 204,
    CartItemRetrievedSuccessfully = 205,
    CartItemRemovedSuccessfully = 206,
    CartItemUpdatedSuccessfully = 207,

    Unauthorized = 300,
    CartInitiatedSuccessfully = 101,
    //Addresses
    DefaultAddressSet = 200,
    AddressNotFound = 201,
    AddressNotOwned = 202,
    NoDefaultAddressFound = 203,
    // user
    UserNotFound = 401,
    Unauthorized = 402,
}
