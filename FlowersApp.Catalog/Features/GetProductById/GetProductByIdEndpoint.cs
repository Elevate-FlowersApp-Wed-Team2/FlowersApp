using FlowersApp.Catalog.Features.GetProductCatalog;
using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Catalog.Features.GetProductById
{
    public class GetProductByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(Endpoints.Catalog.GetProductById, async (
                Guid id,
                Guid? storeId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetProductByIdQuery(id, storeId), cancellationToken);

                return result.Code switch
                {
                    ResultCode.ProductRetrieved => Results.Ok(
                        ApiResponse<ProductDetailsResponse>.Success(
                            value: result.Result,
                            statusCode: HttpStatusCode.OK,
                            message: result.Message)),

                    ResultCode.ProductNotFound => Results.NotFound(
                        ApiResponse<ProductDetailsResponse>.Failure(
                            errors: new List<string>(),
                            statusCode: HttpStatusCode.NotFound,
                            message: result.Message)),

                   
                    ResultCode.StoreNotResolved => Results.BadRequest(
                        ApiResponse<ProductDetailsResponse>.Failure(
                            errors: new List<string>(),
                            statusCode: HttpStatusCode.BadRequest,
                            message: result.Message)),

                    _ => Results.BadRequest(
                        ApiResponse<ProductDetailsResponse>.Failure(
                            errors: new List<string>(),
                            statusCode: HttpStatusCode.BadRequest,
                            message: result.Message))
                };
            })
            .Produces<ApiResponse<ProductDetailsResponse>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<ProductDetailsResponse>>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse<ProductDetailsResponse>>(StatusCodes.Status400BadRequest)
            .WithName("GetProductById")
            .WithTags("Catalog");
        }
    }
}
