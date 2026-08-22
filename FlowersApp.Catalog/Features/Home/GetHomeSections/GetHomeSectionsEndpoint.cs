
using FlowersApp.Catalog.Features.GetCategoryById;
using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using FloweryApp.Api.Features.Home.GetHomeSections;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Catalog.Features.Home.GetHomeSections;

public sealed class GetHomeSectionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(Endpoints.Home.GetHomeSections, async (
            [FromHeader(Name = "Accept-Language")] string acceptLanguage
           , [FromServices] IMediator mediator, CancellationToken cancellationToken
        ) =>
        {
            var result = await mediator.Send(new GetHomeSectionsQuery(acceptLanguage));
            return result.Code switch
            {
                ResultCode.SectionRetrieved => ApiResponse<IReadOnlyList<HomeSectionResponse>>.Success(result.Result, System.Net.HttpStatusCode.OK),
                _ => ApiResponse<IReadOnlyList<HomeSectionResponse>>.Failure(result.Message, System.Net.HttpStatusCode.BadRequest),
            };
        }).Produces<ApiResponse<IReadOnlyList<HomeSectionResponse>>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<IReadOnlyList<HomeSectionResponse>>>(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("GetHomeSections")
            .WithTags("Home"); ;
    }
}
