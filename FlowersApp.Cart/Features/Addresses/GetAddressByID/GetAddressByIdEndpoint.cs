using FlowersApp.Cart.Shared.Constants;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Cart.Features.Addresses.GetAddressByID
{
    public class GetAddressByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(Endpoints.Address.GetAddressById, async (
                Guid id,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetAddressByIdQuery(id), cancellationToken);

                return result.Code switch
                {
                    ResultCode.AddressNotFound => Results.NotFound(
                        ApiResponse<AddressDetailsDto>.Failure(new List<string>(), HttpStatusCode.NotFound, result.Message)),

                    _ => Results.Ok(
                        ApiResponse<AddressDetailsDto>.Success(result.Result, HttpStatusCode.OK, result.Message))
                };
            })
            .RequireAuthorization()
            .Produces<ApiResponse<AddressDetailsDto>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<AddressDetailsDto>>(StatusCodes.Status404NotFound)
            .WithName("GetAddressById")
            .WithTags("Addresses");
        }
    }
}
