using FlowersApp.Cart.Shared.Constants;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using FlowersApp.Cart.Features.AddToCart;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Cart.Features.UpdateCartItem;

public class UpdateCartItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(Endpoints.ShoppingCats.UpdateCartItem, async (
            [FromRoute] Guid id,
            [FromServices] IMediator mediator,
            [FromBody] UpdateCartItemRequestDto request,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateCartItemOrchestrator(id, request.Quantity);
            var result = await mediator.Send(command, cancellationToken);

            return result.Code switch
            {
                ResultCode.CartItemUpdatedSuccessfully =>
                    ApiResponse<CartResponse>.Success(result.Result, System.Net.HttpStatusCode.OK, result.Message),

                ResultCode.Unauthorized =>
                    ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.Unauthorized),

                ResultCode.CartItemNotFound =>
                    ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.NotFound),

                ResultCode.NoValidQuantity =>
                    ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.BadRequest),

                ResultCode.CanNotUpdateCartItem =>
                    ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.InternalServerError),

                _ => ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.BadRequest)
            };
        });
    }
}

// Matches Swagger UpdateCartItemRequestDto
public record UpdateCartItemRequestDto(int Quantity);