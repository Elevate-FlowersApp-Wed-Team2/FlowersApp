using FlowersApp.Catalog.Features.GetProductCatalog;
using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Catalog.Features.GetProductById;

public class GetProductByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(Endpoints.Catalog.GetProductById ,async ([FromRoute]string productId ,IMediator mediator ) =>
        {
            var result = await mediator.Send(new GetProductByIdQuery(productId));
            return result.Code switch
            {
                ResultCode.ProductNotFound => ApiResponse<GetProductByIdResponse>.Failure(result.Message, System.Net.HttpStatusCode.NotFound),
                ResultCode.ProductRetrieved => ApiResponse<GetProductByIdResponse>.Success(result.Result, System.Net.HttpStatusCode.OK, result.Message),
                _ => ApiResponse<GetProductByIdResponse>.Failure(result.Message, System.Net.HttpStatusCode.BadRequest),
            };
        }).Produces<ApiResponse<PagedResult<GetProductByIdResponse>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetProductById")
            .WithTags("Catalog");
    }
}
