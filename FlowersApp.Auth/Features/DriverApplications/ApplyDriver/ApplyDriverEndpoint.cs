using FlowersApp.Auth.Extensions;
using FlowersApp.Auth.Features.DriverApplications.ApplyDriver;
using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.DriverApplications.ApplyDriver;

public class ApplyDriverEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(Endpoints.DriverApplications.Apply, async (
            [FromForm] ApplyDriverOrchestrator command,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);

            return result.Code switch
            {
                ResultCode.ApplicationSubmittedSuccessfully => Results.Created(
                    $"{Endpoints.DriverApplications.GetById}/{result.Result.ApplicationId}",
                    ApiResponse<ApplyDriverOrchestratorResponse>.Success(result.Result, HttpStatusCode.Created)),

                // Duplicate/conflict cases -> 409, per contract
                ResultCode.DriverIsAlreadyExist or
                ResultCode.UserAlreadyApplied => Results.Conflict(
                    ApiResponse<ApplyDriverOrchestratorResponse>.Failure(result.Message)),

                // Everything else (VehicleNotFound, InvalidGender, document-upload
                // failures, FailedToSubmitApplication, and any other/unmapped
                // failure code) -> 400, since the contract for this endpoint only
                // defines 201/400/409/500 and we never want an unmatched code to
                // fall through to the success branch.
                _ => Results.BadRequest(
                    ApiResponse<ApplyDriverOrchestratorResponse>.Failure(result.Message))
            };
        }).Accepts<ApplyDriverOrchestrator>("multipart/form-data")
          .DisableAntiforgery()
          .Produces<ApiResponse<ApplyDriverOrchestratorResponse>>(StatusCodes.Status201Created)
          .Produces<ApiResponse<ApplyDriverOrchestratorResponse>>(StatusCodes.Status400BadRequest)
          .Produces<ApiResponse<ApplyDriverOrchestratorResponse>>(StatusCodes.Status409Conflict)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithName("ApplyAsDriver")
          .WithTags("Drivers");
    }
}