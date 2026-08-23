namespace FlowersApp.Cart.Domain.Entities;

public class ShoppingCartItem:BaseEntity
{
    public Guid CartId { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPriceSnapshot { get; set; }
    public decimal TotalPrice {  get; set; }
    public ShoppingCart? Cart { get; set; }
}
