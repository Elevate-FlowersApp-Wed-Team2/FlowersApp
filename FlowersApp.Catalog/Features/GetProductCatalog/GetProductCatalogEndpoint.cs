using FlowersApp.Catalog.Features.GetProductCatalog;
using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.GetProductCatalog
{
    public class GetProductCatalogEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(Endpoints.Catalog.GetProducts, async (
                [AsParameters] GetProductCatalogQuery query,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(query, cancellationToken);

                return result.Code switch
                {
                    ResultCode.CatalogRetrieved => Results.Ok(
                        ApiResponse<PagedResult<ProductCatalogItemResponse>>.Success(result.Result, HttpStatusCode.OK)),

                    _ => Results.BadRequest(
                        ApiResponse<PagedResult<ProductCatalogItemResponse>>.Failure(result.Message, HttpStatusCode.BadRequest))
                };
            })
            .Produces<ApiResponse<PagedResult<ProductCatalogItemResponse>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("GetProductCatalog")
            .WithTags("Catalog");
        }
    }
}
