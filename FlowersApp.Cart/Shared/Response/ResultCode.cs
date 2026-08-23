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
}
