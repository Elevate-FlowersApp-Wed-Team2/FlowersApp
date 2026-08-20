using FlowersApp.Catalog.Shared.Constants;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Catalog.Features.GetOccasions
{
    public class GetOccasionsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(Endpoints.Occasion.GetOccasions, async (
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetOccasionsQuery(), cancellationToken);

                return result.Code switch
                {
                    ResultCode.OccasionsRetrieved => Results.Ok(
                        ApiResponse<List<OccasionResponse>>.Success(
                            value: result.Result,
                            statusCode: HttpStatusCode.OK,
                            message: result.Message)),

                    _ => Results.BadRequest(
                        ApiResponse<List<OccasionResponse>>.Failure(
                            errors: new List<string>(),
                            statusCode: HttpStatusCode.BadRequest,
                            message: result.Message))
                };
            })
            .Produces<ApiResponse<List<OccasionResponse>>>(StatusCodes.Status200OK)
            .WithName("GetOccasions")
            .WithTags("Catalog");
        }
    }
}
