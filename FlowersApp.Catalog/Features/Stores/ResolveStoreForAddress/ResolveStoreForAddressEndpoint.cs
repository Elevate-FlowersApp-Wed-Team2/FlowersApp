using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Catalog.Features.Stores.ResolveStoreForAddress
{
    public class ResolveStoreForAddressEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost(Endpoints.AddressResolution.ResolveStoreForAddress, async (
                ResolveStoreForAddressQuery query,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(query, cancellationToken);

                return result.Code switch
                {
                    ResultCode.NoCoveringStoreFound => Results.Ok(
                        ApiResponse<Guid?>.Success(null, HttpStatusCode.OK, result.Message)), // resolved but unassigned — not an error

                    _ => Results.Ok(
                        ApiResponse<Guid?>.Success(result.Result, HttpStatusCode.OK, result.Message))
                };
            })
            .Produces<ApiResponse<Guid?>>(StatusCodes.Status200OK)
            .WithName("ResolveStoreForAddress")
            .WithTags("Stores");
        }
    }
}
