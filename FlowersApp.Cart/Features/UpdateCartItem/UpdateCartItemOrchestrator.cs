using FlowersApp.Cart.Features.AddToCart;
using FlowersApp.Cart.Features.GetCartItemById;
using FlowersApp.Cart.Features.GetCartSummary;
using FlowersApp.Cart.Features.GetProductByProductId;
using FlowersApp.Cart.Infrastructure.Persistence;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using MediatR;

namespace FlowersApp.Cart.Features.UpdateCartItem;

public record UpdateCartItemOrchestrator(
    Guid CartItemId,
    int Quantity
) : ICommand<CartResponse>;

public class UpdateCartItemOrchestratorHandler : ICommandHandler<UpdateCartItemOrchestrator, CartResponse>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCartItemOrchestratorHandler> _logger;
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public UpdateCartItemOrchestratorHandler(
        UnitOfWork unitOfWork,
        ILogger<UpdateCartItemOrchestratorHandler> logger,
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public async Task<RequestResult<CartResponse>> Handle(
        UpdateCartItemOrchestrator request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return RequestResult<CartResponse>.Failure(ResultCode.Unauthorized);

        try
        {
            var itemResult = await _mediator.Send(
                new GetCartItemByIdQuery(request.CartItemId),
                cancellationToken);

            if (!itemResult.Success)
                return RequestResult<CartResponse>.Failure(itemResult.Code);

            var productResult = await _mediator.Send(
                new GetProductByProductIdQuery(itemResult.Result!.ProductId),
                cancellationToken);

            if (!productResult.Success)
                return RequestResult<CartResponse>.Failure(productResult.Code);

            if (productResult.Result.StockQuantity < request.Quantity)
                return RequestResult<CartResponse>.Failure(ResultCode.NoValidQuantity);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var updateResult = await _mediator.Send(
                new UpdateCartItemCommand(
                    request.CartItemId, request.Quantity),
                cancellationToken);

            if (!updateResult.Success)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return RequestResult<CartResponse>.Failure(updateResult.Code);
            }

            var affectedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (affectedRows == 0)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return RequestResult<CartResponse>.Failure(ResultCode.CanNotUpdateCartItem);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var cartSummaryResult = await _mediator.Send(
                new GetCartSummaryQuery(userId),
                cancellationToken);

            if (!cartSummaryResult.Success)
                return RequestResult<CartResponse>.Failure(cartSummaryResult.Code);

            return RequestResult<CartResponse>.succeeded(
                cartSummaryResult.Result,
                ResultCode.CartItemUpdatedSuccessfully);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cart item {CartItemId} for user {UserId}",
                request.CartItemId, userId);

            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return RequestResult<CartResponse>.Failure(ResultCode.CanNotUpdateCartItem);
        }
    }
}