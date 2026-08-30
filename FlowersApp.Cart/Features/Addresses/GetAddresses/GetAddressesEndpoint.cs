using FlowersApp.Cart.Shared.Constants;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Cart.Features.Addresses.GetAddresses
{
    public class GetAddressesEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(Endpoints.Address.GetAddresses, async (
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetAddressesQuery(), cancellationToken);

                return Results.Ok(
                    ApiResponse<List<AddressListItemDto>>.Success(result.Result, HttpStatusCode.OK, result.Message));
            })
            .RequireAuthorization()
            .Produces<ApiResponse<List<AddressListItemDto>>>(StatusCodes.Status200OK)
            .WithName("GetAddresses")
            .WithTags("Addresses");
        }
    }
}
