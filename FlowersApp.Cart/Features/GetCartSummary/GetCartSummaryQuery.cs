using FlowersApp.Cart.Domain.Entities;
using FlowersApp.Cart.Features.AddToCart;
using FlowersApp.Cart.Infrastructure.Persistence.Repositories;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Cart.Features.GetCartSummary;

public record GetCartSummaryQuery(string UserId) : IQuery<CartResponse>;

public class GetCartSummaryQueryHandler : IQueryHandler<GetCartSummaryQuery, CartResponse>
{
    private readonly Repository<ShoppingCart> _cartRepository;
    private readonly ILogger<GetCartSummaryQueryHandler> _logger;

    public GetCartSummaryQueryHandler(
        Repository<ShoppingCart> cartRepository,
        ILogger<GetCartSummaryQueryHandler> logger)
    {
        _cartRepository = cartRepository;
        _logger = logger;
    }

    public async Task<RequestResult<CartResponse>> Handle(
        GetCartSummaryQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.UserId))
                return RequestResult<CartResponse>.Failure(ResultCode.Unauthorized);

            // Project directly to CartResponse with items
            var cartData = await _cartRepository
                .Get(c => c.UserId == request.UserId)
                .Select(c => new CartResponse
                (
                    c.Items.Select(i => new CartItemDto(
                        i.Id.ToString(),
                        i.ProductId,
                        i.ProductName,
                        i.ImageUrl,
                        i.UnitPriceSnapshot,
                        i.Quantity,
                        i.UnitPriceSnapshot * i.Quantity,
                         true,
                        (int?)null,
                        false
                    )).ToList(),
                    c.Subtotal,c.DeliveryFee,c.Total,true
                ))
                .FirstOrDefaultAsync(cancellationToken);
            if (cartData == null)
            {
                _logger.LogInformation("No cart found for user {UserId}, returning empty cart", request.UserId);
                return RequestResult<CartResponse>.Failure(ResultCode.NotFoundCartForThisUser);
            }
            
            return RequestResult<CartResponse>.succeeded(cartData,ResultCode.CartRetrivedSuccesfully);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cart summary for user {UserId}", request.UserId);
            return RequestResult<CartResponse>.Failure(ResultCode.CartRetrievalFailed);
        }
    }
}