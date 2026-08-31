using FlowersApp.Cart.Shared.Constants;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Cart.Features.AddToCart;

public class AddToCartEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(Endpoints.ShoppingCats.AddToCart, async (
            [FromServices] IMediator mediator,
            [FromBody] AddToCartorchestrator request,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request, cancellationToken);

            return result.Code switch
            {
                ResultCode.ProductAddedSuccesfully =>
                    ApiResponse<CartResponse>.Success(result.Result, System.Net.HttpStatusCode.OK,result.Message),

                ResultCode.Unauthorized =>
                    ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.Unauthorized),

                ResultCode.CartRetrievalFailed =>
                    ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.BadRequest),

                ResultCode.ProductIsOutOfStock =>
                    ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.BadRequest),

                ResultCode.NoValidQuantity =>
                    ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.BadRequest),

                ResultCode.CanNotUpdateCartItem =>
                    ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.InternalServerError),

                _ => ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.BadRequest)
            };
        });
    }
}