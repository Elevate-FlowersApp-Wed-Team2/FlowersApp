using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Auth.Features.Vehicles.GetVehicleById;



public class GetVehicleByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(Endpoints.Vehicles.GetById, async (
            [FromRoute] Guid id,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetVehicleByIdQuery(id), cancellationToken);

            return result.Code switch
            {
                ResultCode.VehicleRetrieved => Results.Ok(
                    ApiResponse<VehicleDetails>.Success(result.Result,System.Net.HttpStatusCode.OK)),

                ResultCode.VehicleNotFound => Results.NotFound(
                    ApiResponse<VehicleDetails>.Failure(result.Message)),

                _ => Results.BadRequest(
                    ApiResponse<VehicleDetails>.Failure(result.Message))
            };
        })
          .Produces<ApiResponse<VehicleDetails>>(StatusCodes.Status200OK)
          .Produces<ApiResponse<VehicleDetails>>(StatusCodes.Status404NotFound)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithName("GetVehicleById")
          .WithTags("Vehicles");
    }
}