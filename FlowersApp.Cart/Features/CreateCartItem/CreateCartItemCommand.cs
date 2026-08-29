using FlowersApp.Cart.Domain.Entities;
using FlowersApp.Cart.Infrastructure.Persistence.Repositories;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;

namespace FlowersApp.Cart.Features.CreateCartItem;

public record CreateCartItemCommand
(Guid CartId , string ProductId, int Quantity, string ProductName,string ImageUrl,
    decimal UnitPriceSnapshot , decimal? DiscountPercentage ,decimal? DiscountedPrice) : ICommand<bool>;

public class CreateCartItemCommandHandler (Repository<ShoppingCartItem> repository)
    : ICommandHandler<CreateCartItemCommand, bool>
{
    private readonly Repository<ShoppingCartItem> _repository = repository;

    public async Task<RequestResult<bool>> Handle(CreateCartItemCommand request, CancellationToken cancellationToken)
    {
        var cartItem = new ShoppingCartItem
        {
            Id = Guid.NewGuid(),
            CartId = request.CartId,
            ProductId = request.ProductId,
            ProductName = request.ProductName,
            Quantity = request.Quantity,
            TotalPrice = request.Quantity * request.UnitPriceSnapshot,
            UnitPriceSnapshot = request.UnitPriceSnapshot,
            DiscountedPrice = request.DiscountedPrice,
            DiscountPercentage = request.DiscountPercentage,
            ImageUrl = request.ImageUrl,
        };
        _repository.Add(cartItem);
        return RequestResult<bool>.succeeded(true, ResultCode.CartItemCreatedSuccesfully);
    }
}
