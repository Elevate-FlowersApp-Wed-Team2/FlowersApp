using FlowersApp.Auth.Extensions;
using FlowersApp.Auth.Features.DriverApplications.SubmitApplication;
using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Auth.Features.DriverApplications.ApplyDriver;

public class ApplyDriverEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(Endpoints.DriverApplications.Apply, async ([AsParameters] ApplyDriverCommand command,
            [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            //var message = result.Code.Localize(localizer);
            return  result.Code switch
            {
                ResultCode.DriverIsAlreadyExist => Results.Conflict(ApiResponse<ApplyDriverResponse>.Failure(result.Message)),
                ResultCode.VehicleNotFound => Results.NotFound(ApiResponse<ApplyDriverResponse>.Failure(result.Message)),
                ResultCode.FailedToSubmitApplication => Results.BadRequest(ApiResponse<ApplyDriverResponse>.Failure(result.Message)),
                _ => Results.Ok(ApiResponse<ApplyDriverResponse>.Success(result.Result))
            };
        }).Accepts<ApplyDriverCommand>("multipart/form-data")
          .DisableAntiforgery()
          .Produces<ApiResponse<ApplyDriverResponse>>(StatusCodes.Status200OK)
          .Produces<ApiResponse<ApplyDriverResponse>>(StatusCodes.Status400BadRequest)
          .Produces<ApiResponse<ApplyDriverResponse>>(StatusCodes.Status404NotFound)
          .Produces<ApiResponse<ApplyDriverResponse>>(StatusCodes.Status409Conflict)
          .WithName("ApplyDriver")
          .WithTags("DriverApplications");  
    }
}
