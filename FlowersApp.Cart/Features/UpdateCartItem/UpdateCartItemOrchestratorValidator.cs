
using FlowersApp.Cart.Shared.Response;
using FluentValidation;

namespace FlowersApp.Cart.Features.UpdateCartItem;

public class UpdateCartItemOrchestratorValidator : AbstractValidator<UpdateCartItemOrchestrator>
{
    public UpdateCartItemOrchestratorValidator()
    {
        RuleFor(x => x.CartItemId)
            .NotEmpty()
            .WithMessage("Cart item id is required.");

        // Contract: "A quantity of 0 is rejected here; call remove instead."
        // GreaterThan(0) enforces the min:1 + rejects 0 in one rule.
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0. Use the remove endpoint to delete a line.")
            .WithErrorCode(ResultCode.InvalidQuantity.ToString());
    }
}