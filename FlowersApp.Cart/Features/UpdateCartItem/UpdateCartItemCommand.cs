using FlowersApp.Cart.Domain.Entities;
using FlowersApp.Cart.Infrastructure.Persistence.Repositories;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
namespace FlowersApp.Cart.Features.UpdateCartItem;

public record UpdateCartItemCommand
(Guid Id, int Queantity) : ICommand<bool>;

public class UpdateCartItemCommandHandler(Repository<ShoppingCartItem> repository)
    : ICommandHandler<UpdateCartItemCommand, bool>
{
    private readonly Repository<ShoppingCartItem> _repository = repository;

    public async Task<RequestResult<bool>> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        _repository.SaveInclude(new ShoppingCartItem
        {
            Id = request.Id,
            Quantity = request.Queantity
        }, nameof(ShoppingCartItem.Quantity));
        return RequestResult<bool>.succeeded(true, ResultCode.CartItemUpdatedSuccesfully);
        //try
        //{
        //    var affectedRows = await _repository.SaveChangeAsync(cancellationToken);
        //    if (affectedRows == 0)
        //        return RequestResult<bool>.Failure(ResultCode.NoCartItemFounded);
        //    return RequestResult<bool>.succeeded(true,ResultCode.CartItemUpdatedSuccesfully);
        //}
        //catch (Exception ex)
        //{
        //    return RequestResult<bool>.Failure(ResultCode.CanNotUpdateCartItem);
        //}
    }
}
