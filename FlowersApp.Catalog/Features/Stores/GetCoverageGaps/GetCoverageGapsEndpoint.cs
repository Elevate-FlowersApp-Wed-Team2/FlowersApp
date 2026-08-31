using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Catalog.Features.Stores.GetCoverageGaps
{
    public class GetCoverageGapsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost(Endpoints.Store.GetCoverageGaps, async (
                GetCoverageGapsQuery query,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(query, cancellationToken);

                return Results.Ok(
                    ApiResponse<List<CoordinateDto>>.Success(result.Result, HttpStatusCode.OK, result.Message));
            })
            .RequireAuthorization("AdminOnly")
            .Produces<ApiResponse<List<CoordinateDto>>>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .WithName("GetCoverageGaps")
            .WithTags("Stores");
        }
    }
}
