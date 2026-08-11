using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlowersApp.Auth.Features.Auth.UserLogin;

public class UserLoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var handler = async (
            [FromBody] UserLoginOrchestrator command,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);

            return result.Code switch
            {
                ResultCode.LoginSuccessful => Results.Ok(
                    ApiResponse<AuthResponse>.Success(result.Result, HttpStatusCode.OK, result.Message)),

                ResultCode.TooManyFailedAttempts => Results.Json(
                    ApiResponse<AuthResponse>.Failure(result.Message, (HttpStatusCode)StatusCodes.Status429TooManyRequests),
                    statusCode: StatusCodes.Status429TooManyRequests),

                ResultCode.InvalidCredentials or _ => Results.Unauthorized()
            };
        };

        app.MapPost(Endpoints.Auth.UserLogin, handler)
            .Accepts<UserLoginOrchestrator>("application/json")
            .DisableAntiforgery()
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status401Unauthorized)
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status429TooManyRequests)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("UserLogin")
            .WithTags("Auth");

        app.MapPost($"/api/v1/{Endpoints.Auth.UserLogin}", handler)
            .Accepts<UserLoginOrchestrator>("application/json")
            .DisableAntiforgery()
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status401Unauthorized)
            .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status429TooManyRequests)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("UserLoginV1")
            .WithTags("Auth");
    }
}
