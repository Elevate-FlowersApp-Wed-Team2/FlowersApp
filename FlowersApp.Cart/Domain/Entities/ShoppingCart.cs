namespace FlowersApp.Cart.Domain.Entities;

public class ShoppingCart :BaseEntity
{
    public string UserId { get; set; }
    public decimal Total { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal Subtotal { get; set; }
    public List<ShoppingCartItem> Items { get; set; }
}
