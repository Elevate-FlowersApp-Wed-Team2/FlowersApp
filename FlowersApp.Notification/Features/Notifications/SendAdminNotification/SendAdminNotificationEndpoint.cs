using FlowersApp.Notification.Shared.Interfaces;
using FlowersApp.Notification.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Notification.Features.Notifications.SendAdminNotification;

public class SendAdminNotificationEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/notifications/admin", async (
            [FromBody] SendAdminNotificationCommand command,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);

            return result.Code switch
            {
                ResultCode.NotificationSentSuccessfully or ResultCode.NoActiveDevicesFound => Results.Ok(ApiResponse<Guid>.Success(result.Result, message: result.Message)),
                _ => Results.BadRequest(ApiResponse<Guid>.Failure(result.Message))
            };
        })
        .Accepts<SendAdminNotificationCommand>("application/json")
        .Produces<ApiResponse<Guid>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<Guid>>(StatusCodes.Status400BadRequest)
        .WithName("SendAdminNotification")
        .WithTags("Notifications");
    }
}
