using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.Vehicles.CreateVehicle;
public class CreateVehicleEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(Endpoints.Vehicles.Create, async (
            [FromBody] CreateVehicleCommand command,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);

            return result.Code switch
            {
                ResultCode.VehicleCreated => Results.Created(
                    $"{Endpoints.Vehicles.Base}/{result.Result.Id}",
                    ApiResponse<CreateVehicleResponse>.Success(result.Result, HttpStatusCode.Created)),

                ResultCode.VehicleAlreadyExists => Results.Conflict(
                    ApiResponse<CreateVehicleResponse>.Failure(result.Message)),

                _ => Results.BadRequest(
                    ApiResponse<CreateVehicleResponse>.Failure(result.Message))
            };
        })
          .Produces<ApiResponse<CreateVehicleResponse>>(StatusCodes.Status201Created)
          .Produces<ApiResponse<CreateVehicleResponse>>(StatusCodes.Status400BadRequest)
          .Produces<ApiResponse<CreateVehicleResponse>>(StatusCodes.Status409Conflict)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithName("CreateVehicle")
          .WithTags("Vehicles");
    }
}