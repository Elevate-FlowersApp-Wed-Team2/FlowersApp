using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Catalog.Features.GetCategories
{
    public class GetCategoriesEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(Endpoints.Category.GetCategories, async (
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetCategoriesQuery(), cancellationToken);

                return result.Code switch
                {
                    ResultCode.CategoriesRetrieved => Results.Ok(
                        ApiResponse<List<CategoryResponse>>.Success(result.Result, HttpStatusCode.OK, result.Message)),

                    _ => Results.BadRequest(
                        ApiResponse<List<CategoryResponse>>.Failure(result.Message, HttpStatusCode.BadRequest))
                };
            })
            .Produces<ApiResponse<List<CategoryResponse>>>(StatusCodes.Status200OK)
            .WithName("GetCategories")
            .WithTags("Catalog");
        }
    }
}
