using FlowersApp.Cart.Shared.Constants;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Cart.Features.Addresses.GetDefaultAddress
{
    public class GetDefaultAddressEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(Endpoints.Address.GetDefaultAddress, async (
                [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetDefaultAddressQuery(), cancellationToken);

                return Results.Ok(ApiResponse<DefaultAddressDto?>.Success(result.Result, HttpStatusCode.OK, result.Message));
            })
            .RequireAuthorization()
            .Produces<ApiResponse<DefaultAddressDto?>>(StatusCodes.Status200OK)
            .WithName("GetDefaultAddress")
            .WithTags("Addresses");
        }
    }
}
