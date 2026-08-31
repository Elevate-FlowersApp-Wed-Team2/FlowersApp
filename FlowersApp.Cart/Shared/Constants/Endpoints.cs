namespace FlowersApp.Cart.Shared.Constants;

public static class Endpoints
{
    private const string BasePath = "api/v1";
    public static class Address
    {
        private const string Base = $"{BasePath}/addresses";

        public const string GetAddresses = Base;
        public const string GetAddressById = $"{Base}/{{id}}";
        public const string SetDefaultAddress = $"{Base}/{{id}}/default";
        public const string GetDefaultAddress = $"{Base}/default";
    }


    public static class ShoppingCats
    {
        public const string BaseCart = $"{BasePath}/Cart";
        public const string GetCart = $"{BasePath}/Cart";
        public const string AddToCart = $"{BaseCart}/Items";
        public const string UpdateCartItem = $"{BaseCart}/Items/{{id}}";
        public const string DeleteCartItem = $"{BaseCart}/Items/{{id}}/{{userId}}";
    }
    
}