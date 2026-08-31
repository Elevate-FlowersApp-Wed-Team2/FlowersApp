using FlowersApp.Cart.Features.AddToCart;
using FlowersApp.Cart.Features.DeleteCartItem;
using FlowersApp.Cart.Features.GetCartItemById;
using FlowersApp.Cart.Features.GetCartSummary;
using FlowersApp.Cart.Infrastructure.Persistence;
using FlowersApp.Cart.Shared.Constants;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using MediatR;

namespace FlowersApp.Cart.Features.RemoveCartItem;

public record RemoveCartItemOrchestrator(Guid CartItemId,string UserId) : ICommand<CartResponse>;

public class RemoveCartItemOrchestratorHandler : ICommandHandler<RemoveCartItemOrchestrator, CartResponse>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ILogger<RemoveCartItemOrchestratorHandler> _logger;
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public RemoveCartItemOrchestratorHandler(
        UnitOfWork unitOfWork,
        ILogger<RemoveCartItemOrchestratorHandler> logger,
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    }

    public async Task<RequestResult<CartResponse>> Handle(
        RemoveCartItemOrchestrator request,
        CancellationToken cancellationToken)
    {
        //var userId = _currentUserService.UserId;
        //if (string.IsNullOrEmpty(userId))
        //    return RequestResult<CartResponse>.Failure(ResultCode.Unauthorized);

        try
        {
            var deleteResult = await _mediator.Send(
                new DeleteCartItemCommand(request.CartItemId),
                cancellationToken);

            if (!deleteResult.Success)
                return RequestResult<CartResponse>.Failure(deleteResult.Code);
            
            // Step 3: Return the recalculated cart (client's "Undo" is just re-adding the line)
            var cartSummaryResult = await _mediator.Send(
                new GetCartSummaryQuery(request.UserId),
                cancellationToken);

            if (!cartSummaryResult.Success)
                return RequestResult<CartResponse>.Failure(cartSummaryResult.Code);

            return RequestResult<CartResponse>.succeeded(
                cartSummaryResult.Result,
                ResultCode.CartItemRemovedSuccessfully);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cart item {CartItemId} for user {UserId}",
                request.CartItemId, request.UserId);
            return RequestResult<CartResponse>.Failure(ResultCode.CanNotUpdateCartItem);
        }
    }
}