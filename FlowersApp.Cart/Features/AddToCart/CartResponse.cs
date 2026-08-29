namespace FlowersApp.Cart.Features.AddToCart;

public record CartResponse(
    List<CartItemDto> Items,
    decimal Subtotal,
    decimal? DeliveryFee,
    decimal Total,
    bool HasChanges
);

// Matches Swagger CartItem schema
public record CartItemDto(
    string Id,                    // Cart line ID (UUID)
    string ProductId,                // Product ID (int)
    string ProductName,
    string ProductImageUrl,
    decimal UnitPrice,            // Current price
    int Quantity,
    decimal LineSubtotal,         // unitPrice * quantity
    bool InStock,
    int? AvailableStock,          // Remaining stock
    bool PriceChanged             // Price changed since added
);