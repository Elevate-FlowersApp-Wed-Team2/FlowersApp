using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using FluentValidation;

namespace FlowersApp.Cart.Features.AddToCart;

public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
{
    private readonly ICurrentUserService _currentUserService;

    public AddToCartCommandValidator(ICurrentUserService currentUserService)
    {
        // Validate ProductId
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than 0.")
            .WithErrorCode(ResultCode.InvalidProductId.ToString());

        // Validate Quantity
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.")
            .WithErrorCode(ResultCode.InvalidQuantity.ToString());

        // Custom validation for product existence and stock will be handled in handler
    }
}
