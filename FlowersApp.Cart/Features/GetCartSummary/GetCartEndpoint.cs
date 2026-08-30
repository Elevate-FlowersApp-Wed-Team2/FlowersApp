using FlowersApp.Cart.Features.AddToCart;
using FlowersApp.Cart.Features.GetCartSummary;
using FlowersApp.Cart.Shared.Constants;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Cart.Features.GetCart;

public class GetCartEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(Endpoints.ShoppingCats.GetCart, async ([FromRoute]string userId,
            [FromServices] IMediator mediator,
            [FromServices] ICurrentUserService currentUserService,
            CancellationToken cancellationToken) =>
        {
            //var userId = currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ApiResponse<CartResponse>.Failure("Unauthorized", System.Net.HttpStatusCode.Unauthorized);

            var result = await mediator.Send(new GetCartSummaryQuery(userId), cancellationToken);

            if (!result.Success)
                return ApiResponse<CartResponse>.Failure(result.Message, System.Net.HttpStatusCode.InternalServerError);

            return ApiResponse<CartResponse>.Success(result.Result, System.Net.HttpStatusCode.OK, result.Message);
        });
    }
}