namespace FlowersApp.Cart.Domain.Entities;

public class ShoppingCart :BaseEntity
{
    public string UserId { get; set; }
    public List<ShoppingCartItem> Items { get; set; }
}
