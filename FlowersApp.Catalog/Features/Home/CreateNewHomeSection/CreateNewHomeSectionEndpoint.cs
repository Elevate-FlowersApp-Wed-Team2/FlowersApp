using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using FloweryApp.Api.Features.Home.GetHomeSectionsForAdmin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Catalog.Features.Home.CreateNewHomeSection;

public class CreateNewHomeSectionEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(Endpoints.Admin.CreateNewSection, async ([FromServices]IMediator mediator,
            [FromBody]CreateNewHomeSectionCommand request , CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request, cancellationToken);
            return result.Code switch
            {
                ResultCode.SectionSavedSuccesfully => ApiResponse<CreateNewHomeSectionResponse>.Success(result.Result,System.Net.HttpStatusCode.Created,result.Message),
                _ => ApiResponse<CreateNewHomeSectionResponse>.Failure(result.Message,System.Net.HttpStatusCode.BadRequest,result.Message),
            };
        })
        .Produces<ApiResponse<CreateNewHomeSectionResponse>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<CreateNewHomeSectionResponse>>(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithName("CreateHomeSection")
        .WithTags("Admin");
    }
}
