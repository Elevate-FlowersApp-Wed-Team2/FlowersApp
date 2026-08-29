using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Catalog.Features.Stores.CreateStore
{
    public class CreateStoreEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost(Endpoints.Store.CreateStore, async (
                CreateStoreCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                return result.Code switch
                {
                    ResultCode.StoreCreated => Results.Ok(
                        ApiResponse<Guid>.Success(result.Result, HttpStatusCode.Created, result.Message)),

                    _ => Results.BadRequest(
                        ApiResponse<Guid>.Failure(new List<string>(), HttpStatusCode.BadRequest, result.Message))
                };
            })
            .RequireAuthorization("AdminOnly")
            .Produces<ApiResponse<Guid>>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .WithName("CreateStore")
            .WithTags("Stores");
        }
    }
}
