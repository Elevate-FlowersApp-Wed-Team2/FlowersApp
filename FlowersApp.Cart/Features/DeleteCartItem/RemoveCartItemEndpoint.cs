using FlowersApp.Cart.Features.AddToCart;
using FlowersApp.Cart.Shared.Constants;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Cart.Features.RemoveCartItem;

public class RemoveCartItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(Endpoints.ShoppingCats.DeleteCartItem, async (
            [FromRoute] Guid id, [FromRoute] string userId,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new RemoveCartItemOrchestrator(id,userId), cancellationToken);

            return result.Code switch
            {
                ResultCode.CartItemRemovedSuccessfully =>
                    ApiResponse<CartResponse>.Success(result.Result, System.Net.HttpStatusCode.OK, result.Message),

                ResultCode.Unauthorized =>
                    ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.Unauthorized),

                ResultCode.CartItemNotFound =>
                    ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.NotFound),

                ResultCode.CanNotUpdateCartItem =>
                    ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.InternalServerError),

                _ => ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.BadRequest)
            };
        });
    }
}