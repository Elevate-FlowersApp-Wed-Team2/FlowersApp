using FlowersApp.Cart.Domain.Entities;
using FlowersApp.Cart.Features.CreateCartItem;
using FlowersApp.Cart.Features.GetCartSummary;
using FlowersApp.Cart.Features.GetProductByProductId;
using FlowersApp.Cart.Features.GetUserCartWithSpecificProduct;
using FlowersApp.Cart.Features.UpdateCartItem;
using FlowersApp.Cart.Infrastructure.Persistence;
using FlowersApp.Cart.Infrastructure.Persistence.Repositories;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace FlowersApp.Cart.Features.AddToCart;

public record AddToCartCommand(
    string ProductId,
    int Quantity,
    string UserId
) : ICommand<CartResponse>;

public class AddToCartCommandHandler : ICommandHandler<AddToCartCommand, CartResponse>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ILogger<AddToCartCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public AddToCartCommandHandler(
        UnitOfWork unitOfWork,
        ILogger<AddToCartCommandHandler> logger,
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    }

    public async Task<RequestResult<CartResponse>> Handle(
        AddToCartCommand request,
        CancellationToken cancellationToken)
    {
        // Get authenticated user ID
        //var userId = _currentUserService.UserId;
        //if (string.IsNullOrEmpty(userId))
        //    return RequestResult<CartResponse>.Failure(ResultCode.Unauthorized);
        var userId = request.UserId;

        try
        {
            // Step 1: Get user's cart with specific product
            var cartResult = await _mediator.Send(
                new GetUserCartWithSpecificProductQuery(userId, request.ProductId.ToString()),
                cancellationToken);

            if (!cartResult.Success)
                return RequestResult<CartResponse>.Failure(cartResult.Code);

            // Step 2: Get product details for validation
            var productResult = await _mediator.Send(
                new GetProductByProductIdQuery(request.ProductId.ToString()),
                cancellationToken);

            if (!productResult.Success)
                return RequestResult<CartResponse>.Failure(productResult.Code);

            // Step 3: Validate product availability
            if (productResult.Result.IsOutOfStock)
                return RequestResult<CartResponse>.Failure(ResultCode.ProductIsOutOfStock);

            // Step 4: Calculate new quantity (increment if exists)
            var existingQuantity = cartResult.Result?.ProductData?.Quentity ?? 0;
            var newQuantity = existingQuantity + request.Quantity;

            // Step 5: Validate against stock
            if (productResult.Result.StockQuantity < newQuantity)
                return RequestResult<CartResponse>.Failure(ResultCode.NoValidQuantity);

            // Step 6: Begin transaction
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // Step 7: Update or create cart item
            if (cartResult.Result?.ProductData != null)
            {
                // Update existing item (increment quantity)
                var updateResult = await _mediator.Send(
                    new UpdateCartItemCommand(
                        cartResult.Result.ProductData.CartItemId,
                        newQuantity),
                    cancellationToken);

                if (!updateResult.Success)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return RequestResult<CartResponse>.Failure(updateResult.Code);
                }
            }
            else
            {
                // Create new cart item
                var createResult = await _mediator.Send(
                    new CreateCartItemCommand(
                        cartResult.Result!.CartId,
                        request.ProductId.ToString(),
                        request.Quantity,
                        productResult.Result.Name,
                        productResult.Result.ImageUrls.FirstOrDefault(),
                        productResult.Result.OriginalPrice,
                        productResult.Result.DiscountPercentage,
                        productResult.Result.DiscountedPrice),
                    cancellationToken);

                if (!createResult.Success)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return RequestResult<CartResponse>.Failure(createResult.Code);
                }
            }

            // Step 8: Save changes
            var affectedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (affectedRows == 0)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return RequestResult<CartResponse>.Failure(ResultCode.CanNotUpdateCartItem);
            }

            // Step 9: Commit transaction
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            // Step 10: Get updated cart summary (matching Swagger CartResponse)
            var cartSummaryResult = await _mediator.Send(
                new GetCartSummaryQuery(userId),
                cancellationToken);

            if (!cartSummaryResult.Success)
                return RequestResult<CartResponse>.Failure(cartSummaryResult.Code);

            // Step 11: Return cart response (matches Swagger)
            return RequestResult<CartResponse>.succeeded(
                cartSummaryResult.Result,
                ResultCode.ProductAddedSuccesfully);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding product {ProductId} to cart for user {UserId}",
                request.ProductId, userId);

            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return RequestResult<CartResponse>.Failure(ResultCode.CanNotUpdateCartItem);
        }
    }
}
