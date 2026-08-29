using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Catalog.Features.Stores.UpdateStoreCoverage
{
    public class UpdateStoreCoverageEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut(Endpoints.Store.UpdateStoreCoverage, async (
                Guid id,
                UpdateStoreCoverageCommand body,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = body with { StoreId = id };
                var result = await mediator.Send(command, cancellationToken);

                return result.Code switch
                {
                    ResultCode.CoverageUpdated => Results.Ok(
                        ApiResponse<Guid>.Success(result.Result, HttpStatusCode.OK, result.Message)),

                    ResultCode.StoreNotFound => Results.NotFound(
                        ApiResponse<Guid>.Failure(new List<string>(), HttpStatusCode.NotFound, result.Message)),

                    // AC #5 — invalid polygon is a distinct outcome from a generic bad request,
                    // so the frontend can point the admin at the shape they drew rather than
                    // a vague validation error.
                    ResultCode.InvalidCoveragePolygon => Results.BadRequest(
                        ApiResponse<Guid>.Failure(new List<string>(), HttpStatusCode.BadRequest, result.Message)),

                    _ => Results.BadRequest(
                        ApiResponse<Guid>.Failure(new List<string>(), HttpStatusCode.BadRequest, result.Message))
                };
            })
            .RequireAuthorization("AdminOnly")
            .Produces<ApiResponse<Guid>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<Guid>>(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .WithName("UpdateStoreCoverage")
            .WithTags("Stores");
        }
    }
}
