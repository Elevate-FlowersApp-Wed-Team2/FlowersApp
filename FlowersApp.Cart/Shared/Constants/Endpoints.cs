namespace FlowersApp.Cart.Shared.Constants;

public static class Endpoints
{
    private const string BasePath = "api/v1";

    public static class ShoppingCats
    {
        public const string BaseCart = $"{BasePath}/Cart";
        public const string GetCart = $"{BasePath}/Cart";
        public const string AddToCart = $"{BaseCart}/Items";
        public const string UpdateCartItem = $"{BaseCart}/Items/{{id}}";
        public const string DeleteCartItem = $"{BaseCart}/Items/{{id}}/{{userId}}";
    }
    
}