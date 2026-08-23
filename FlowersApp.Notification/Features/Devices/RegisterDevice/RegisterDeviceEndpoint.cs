using FlowersApp.Notification.Shared.Interfaces;
using FlowersApp.Notification.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Notification.Features.Devices.RegisterDevice;

public class RegisterDeviceEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/devices/register", async (
            [FromBody] RegisterDeviceCommand command,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);

            return result.Code switch
            {
                ResultCode.DeviceRegisteredSuccessfully => Results.Ok(ApiResponse<Guid>.Success(result.Result, message: result.Message)),
                _ => Results.BadRequest(ApiResponse<Guid>.Failure(result.Message))
            };
        })
        .Accepts<RegisterDeviceCommand>("application/json")
        .Produces<ApiResponse<Guid>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<Guid>>(StatusCodes.Status400BadRequest)
        .WithName("RegisterDevice")
        .WithTags("Devices");
    }
}
