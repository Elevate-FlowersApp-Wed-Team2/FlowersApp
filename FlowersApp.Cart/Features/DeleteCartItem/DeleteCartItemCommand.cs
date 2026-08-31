using FlowersApp.Cart.Domain.Entities;
using FlowersApp.Cart.Infrastructure.Persistence;
using FlowersApp.Cart.Infrastructure.Persistence.Repositories;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;

namespace FlowersApp.Cart.Features.DeleteCartItem;

public record DeleteCartItemCommand(Guid CartItemId) : ICommand<bool>;

public class DeleteCartItemCommandHandler : ICommandHandler<DeleteCartItemCommand, bool>
{
    private readonly Repository<ShoppingCartItem> _repository;

    public DeleteCartItemCommandHandler(Repository<ShoppingCartItem> repository)
    {
        _repository = repository;
    }

    public async Task<RequestResult<bool>> Handle(
        DeleteCartItemCommand request,
        CancellationToken cancellationToken)
    {
        // NOTE: adjust to your actual repository's remove method/signature.
        _repository.SaveInclude(new ShoppingCartItem
        {
            Id = request.CartItemId,
            IsDeleted = true
        }, nameof(ShoppingCartItem.IsDeleted));

        var affectedRows = await _repository.SaveChangeAsync(cancellationToken);

        return affectedRows > 0
            ? RequestResult<bool>.succeeded(true, ResultCode.CartItemRemovedSuccessfully)
            : RequestResult<bool>.Failure(ResultCode.CanNotUpdateCartItem);
    }
}