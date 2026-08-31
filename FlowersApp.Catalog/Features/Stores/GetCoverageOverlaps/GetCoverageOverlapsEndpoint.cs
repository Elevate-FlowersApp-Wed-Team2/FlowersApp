using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Catalog.Features.Stores.GetCoverageOverlaps
{
    public class GetCoverageOverlapsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(Endpoints.Store.GetCoverageOverlaps, async (
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetCoverageOverlapsQuery(), cancellationToken);

                return Results.Ok(
                    ApiResponse<List<OverlapPairDto>>.Success(result.Result, HttpStatusCode.OK, result.Message));
            })
            .RequireAuthorization("AdminOnly")
            .Produces<ApiResponse<List<OverlapPairDto>>>(StatusCodes.Status200OK)
            .WithName("GetCoverageOverlaps")
            .WithTags("Stores");
        }
    }
}
