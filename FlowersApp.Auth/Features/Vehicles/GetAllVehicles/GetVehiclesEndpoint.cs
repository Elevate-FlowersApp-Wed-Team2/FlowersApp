using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Auth.Features.Vehicles.GetAllVehicles;

public class GetVehiclesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(Endpoints.Vehicles.GetAll, async (
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetVehiclesQuery(), cancellationToken);

            return result.Code switch
            {
                ResultCode.VehiclesRetrieved => Results.Ok(
                    ApiResponse<List<VehicleListItem>>.Success(result.Result ,System.Net.HttpStatusCode.OK)),

                _ => Results.BadRequest(
                    ApiResponse<List<VehicleListItem>>.Failure(result.Message))
            };
        })
          .Produces<ApiResponse<List<VehicleListItem>>>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithName("GetVehicles")
          .WithTags("Vehicles");
    }
}