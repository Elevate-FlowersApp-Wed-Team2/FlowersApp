using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Response;
using FlowersApp.Auth.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Auth.Features.CustomerRegister;

public class CustomerRegisterEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(Endpoints.Auth.Register, async (
            [FromBody] CustomerRegisterCommand request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request, cancellationToken);
            return result.Code switch
            {
                ResultCode.RegistrationSuccessful => Results.Created($"{Endpoints.Auth.Register}/{result.Result}",
                    ApiResponse<Guid>.Success(result.Result!, statusCode: System.Net.HttpStatusCode.Created, result.Message)),
                ResultCode.EmailAlreadyRegistered => Results.Conflict(
                    ApiResponse<Guid>.Failure(result.Message ?? string.Empty, System.Net.HttpStatusCode.Conflict)),
                ResultCode.PhoneAlreadyRegistered => Results.Conflict(
                    ApiResponse<Guid>.Failure(result.Message ?? string.Empty, System.Net.HttpStatusCode.Conflict)),
                _ => Results.BadRequest(ApiResponse<Guid>.Failure(result.Message ?? string.Empty))
            };
        })
        .Accepts<CustomerRegisterCommand>("application/json")
        .DisableAntiforgery()
        .Produces<ApiResponse<Guid>>(StatusCodes.Status201Created)
        .Produces<ApiResponse<Guid>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<Guid>>(StatusCodes.Status409Conflict)
        .WithName("Register")
        .WithTags("Auth");
    }
}
