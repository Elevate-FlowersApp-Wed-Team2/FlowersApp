using FlowersApp.Cart.Shared.Constants;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Cart.Features.Addresses.SetDefaultAddress
{
    public class SetDefaultAddressEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut(Endpoints.Address.SetDefaultAddress, async (
                Guid id,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new SetDefaultAddressCommand(id), cancellationToken);

                return result.Code switch
                {
                    ResultCode.DefaultAddressSet => Results.Ok(
                        ApiResponse<bool>.Success(result.Result, HttpStatusCode.OK, result.Message)),

                    ResultCode.AddressNotFound => Results.NotFound(
                        ApiResponse<bool>.Failure(new List<string>(), HttpStatusCode.NotFound, result.Message)),

                    
                    ResultCode.AddressNotOwned => Results.Json(
                        ApiResponse<bool>.Failure(new List<string>(), HttpStatusCode.Forbidden, result.Message),
                        statusCode: StatusCodes.Status403Forbidden),

                    _ => Results.BadRequest(
                        ApiResponse<bool>.Failure(new List<string>(), HttpStatusCode.BadRequest, result.Message))
                };
            })
            .RequireAuthorization()
            .Produces<ApiResponse<bool>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<bool>>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse<bool>>(StatusCodes.Status403Forbidden)
            .WithName("SetDefaultAddress")
            .WithTags("Addresses");
        }
    }
}
