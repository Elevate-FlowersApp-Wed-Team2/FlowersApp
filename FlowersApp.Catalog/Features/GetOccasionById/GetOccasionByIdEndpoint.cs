using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Catalog.Features.GetOccasionById
{
    public class GetOccasionByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(Endpoints.Occasion.GetOccasionById, async (
                Guid id,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetOccasionByIdQuery(id), cancellationToken);

                return result.Code switch
                {
                    ResultCode.OccasionRetrieved => Results.Ok(
                        ApiResponse<OccasionDetailsResponse>.Success(
                            value: result.Result,
                            statusCode: HttpStatusCode.OK,
                            message: result.Message)),

                    ResultCode.OccasionNotFound => Results.NotFound(
                        ApiResponse<OccasionDetailsResponse>.Failure(
                            errors: new List<string>(),
                            statusCode: HttpStatusCode.NotFound,
                            message: result.Message)),

                    ResultCode.OccasionArchived => Results.BadRequest(
                        ApiResponse<OccasionDetailsResponse>.Failure(
                            errors: new List<string>(),
                            statusCode: HttpStatusCode.Gone,
                            message: result.Message)),

                    _ => Results.BadRequest(
                        ApiResponse<OccasionDetailsResponse>.Failure(
                            errors: new List<string>(),
                            statusCode: HttpStatusCode.BadRequest,
                            message: result.Message))
                };
            })
            .Produces<ApiResponse<OccasionDetailsResponse>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<OccasionDetailsResponse>>(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status410Gone)
            .WithName("GetOccasionById")
            .WithTags("Catalog");
        }
    }
}
