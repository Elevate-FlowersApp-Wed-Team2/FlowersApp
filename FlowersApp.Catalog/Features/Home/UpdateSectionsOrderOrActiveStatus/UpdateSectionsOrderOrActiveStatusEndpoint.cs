using FlowersApp.Catalog.Features.Home.CreateNewHomeSection;
using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Catalog.Features.Home.UpdateSectionsOrderOrActiveStatus;

public class UpdateSectionsOrderOrActiveStatusEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(Endpoints.Admin.UpdateSectionsOrderOrActive , async([FromServices]IMediator mediator
            ,[FromBody] UpdateSectionsOrderOrActiveStatusCommand request) =>
        {
            var result = await mediator.Send(request);
            return result.Code switch
            {
                ResultCode.SectionsUpdatedSuccesfully => ApiResponse<List<UpdateSectionOrderOrActiveStatusResponse>>.Success(result.Result, System.Net.HttpStatusCode.OK, result.Message),
                _ => ApiResponse<List<UpdateSectionOrderOrActiveStatusResponse>>.Failure(result.Message, System.Net.HttpStatusCode.BadRequest, result.Message),
            };

        }).Produces<ApiResponse<List<UpdateSectionOrderOrActiveStatusResponse>>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<List<UpdateSectionOrderOrActiveStatusResponse>>>(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithName("UpdateSections")
        .WithTags("Admin");
    }
}
