
using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using FloweryApp.Api.Features.Home.GetHomeSectionsForAdmin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Catalog.Features.Home.GetHomeSectionsForAdmin;

public sealed class GetHomeSectionsForAdminEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(Endpoints.Admin.GetHomeSections, async ([FromServices] IMediator mediator, CancellationToken cancellationToken
        ) =>
        {
            var result = await mediator.Send(new GetHomeSectionsForAdminQuery());
            return result.Code switch
            {
                ResultCode.SectionRetrieved => ApiResponse<IReadOnlyList<GetHomeSectionsForAdminResponse>>.Success(result.Result, System.Net.HttpStatusCode.OK ,result.Message),
                _ => ApiResponse<IReadOnlyList<GetHomeSectionsForAdminResponse>>.Failure(result.Message, System.Net.HttpStatusCode.BadRequest,result.Message),
            };
        }).Produces<ApiResponse<IReadOnlyList<GetHomeSectionsForAdminResponse>>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<IReadOnlyList<GetHomeSectionsForAdminResponse>>>(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("GetHomeSectionsForAdmin")
            .WithTags("Admin"); ;
    }
}
