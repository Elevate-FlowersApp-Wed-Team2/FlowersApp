using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Catalog.Features.Stores.DeactivateStore
{
    public class DeactivateStoreEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost(Endpoints.Store.DeactivateStore, async (
                Guid id,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new DeactivateStoreCommand(id), cancellationToken);

                return result.Code switch
                {
                    ResultCode.StoreDeactivated => Results.Ok(
                        ApiResponse<bool>.Success(result.Result, HttpStatusCode.OK, result.Message)),

                    ResultCode.StoreNotFound => Results.NotFound(
                        ApiResponse<bool>.Failure(new List<string>(), HttpStatusCode.NotFound, result.Message)),

                    _ => Results.BadRequest(
                        ApiResponse<bool>.Failure(new List<string>(), HttpStatusCode.BadRequest, result.Message))
                };
            })
            .RequireAuthorization("AdminOnly")
            .Produces<ApiResponse<bool>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<bool>>(StatusCodes.Status404NotFound)
            .WithName("DeactivateStore")
            .WithTags("Stores");
        }
    }
}
