using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Catalog.Features.GetCategoryById
{
    public class GetCategoryByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(Endpoints.Category.GetCategoryById, async (
                Guid id,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);

                return result.Code switch
                {
                    ResultCode.CategoryRetrieved => Results.Ok(
                        ApiResponse<CategoryDetailsResponse>.Success(result.Result, HttpStatusCode.OK, result.Message)),

                    // AC #5 — distinct outcomes so the frontend can show a "no longer
                    // available" message specifically for archived, vs a generic
                    // not-found for a bad/stale id.
                    ResultCode.CategoryNotFound => Results.NotFound(
                        ApiResponse<CategoryDetailsResponse>.Failure(new List<string>(), HttpStatusCode.NotFound, result.Message)),

                    ResultCode.CategoryArchived => Results.BadRequest(
                        ApiResponse<CategoryDetailsResponse>.Failure(new List<string>(), HttpStatusCode.Gone, result.Message)),

                    _ => Results.BadRequest(
                        ApiResponse<CategoryDetailsResponse>.Failure(new List<string>(), HttpStatusCode.BadRequest, result.Message))
                };
            })
            .Produces<ApiResponse<CategoryDetailsResponse>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<CategoryDetailsResponse>>(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status410Gone)
            .WithName("GetCategoryById")
            .WithTags("Catalog");
        }
    }
}
