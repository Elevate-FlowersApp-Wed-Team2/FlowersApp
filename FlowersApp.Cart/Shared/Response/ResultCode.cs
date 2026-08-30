namespace FlowersApp.Cart.Shared.Response;

public enum ResultCode
{
    //Cart
    FailedToInitiateCart = 100,
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
