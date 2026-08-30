using FlowersApp.Cart.Domain.Entities;
using FlowersApp.Cart.Infrastructure.Persistence;
using FlowersApp.Cart.Infrastructure.Persistence.Repositories;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Cart.Features.GetCartItemById;

public record CartItemDetails(
    string CartItemId,
    string CartId,
    string ProductId,
    int Quantity
);

public record GetCartItemByIdQuery( Guid CartItemId) : IQuery<CartItemDetails>;

public class GetCartItemByIdQueryHandler : IQueryHandler<GetCartItemByIdQuery, CartItemDetails>
{
    private readonly Repository<ShoppingCartItem> _repository;

    public GetCartItemByIdQueryHandler(Repository<ShoppingCartItem> repository)
    {
        _repository = repository;
    }

    public async Task<RequestResult<CartItemDetails>> Handle(
        GetCartItemByIdQuery request,
        CancellationToken cancellationToken)
    {

        var item = await _repository.Get(s => s.Id == request.CartItemId)
            .Select(c => new CartItemDetails
            (
                c.Id.ToString(),c.CartId.ToString(),c.ProductId,c.Quantity
            ))
            .FirstOrDefaultAsync( cancellationToken);

        if (item is null)
            return RequestResult<CartItemDetails>.Failure(ResultCode.CartItemNotFound);

        return RequestResult<CartItemDetails>.succeeded(
            item,
            ResultCode.CartItemRetrievedSuccessfully);
    }
}
