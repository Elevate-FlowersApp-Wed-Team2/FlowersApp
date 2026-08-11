using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.Auth.DriverLogin;

public class DriverLoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var handler = async (
            [FromBody] DriverLoginOrchestrator command,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);

            return result.Code switch
            {
                ResultCode.LoginSuccessful => Results.Ok(
                    ApiResponse<AuthResponse>.Success(result.Result, HttpStatusCode.OK, result.Message)),

                ResultCode.DriverAccountNotApproved or ResultCode.DriverApplicationRejected => Results.Json(
                    ApiResponse<AuthResponse>.Failure(result.Message, HttpStatusCode.Forbidden),
                    statusCode: StatusCodes.Status403Forbidden),

                ResultCode.TooManyFailedAttempts => Results.Json(
                    ApiResponse<AuthResponse>.Failure(result.Message, (HttpStatusCode)StatusCodes.Status429TooManyRequests),
                    statusCode: StatusCodes.Status429TooManyRequests),

                ResultCode.InvalidCredentials or _ => Results.Unauthorized()
            };
        };

        app.MapPost(Endpoints.Auth.DriverLogin, handler)
            .Accepts<DriverLoginOrchestrator>("application/json")
            .DisableAntiforgery()
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status401Unauthorized)
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status403Forbidden)
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status429TooManyRequests)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("DriverLogin")
            .WithTags("Auth");

        app.MapPost($"/api/v1/{Endpoints.Auth.DriverLogin}", handler)
            .Accepts<DriverLoginOrchestrator>("application/json")
            .DisableAntiforgery()
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status401Unauthorized)
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status403Forbidden)
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status429TooManyRequests)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("DriverLoginV1")
            .WithTags("Auth");
    }
}
