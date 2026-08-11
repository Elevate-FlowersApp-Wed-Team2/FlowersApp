using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Response;
using FlowersApp.Auth.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Auth.Features.Login;

public class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (
            [FromBody] LoginCommand request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request, cancellationToken);
            return result.Code switch
            {
                ResultCode.LoginSuccessful => Results.Ok(ApiResponse<AuthResponse>.Success(result.Result!, System.Net.HttpStatusCode.OK, result.Message)),
                ResultCode.InvalidEmailOrPassword => Results.Json(ApiResponse<AuthResponse>.Failure(result.Message ?? string.Empty, System.Net.HttpStatusCode.Unauthorized), statusCode: StatusCodes.Status401Unauthorized),
                ResultCode.LoginRateLimited => Results.Json(ApiResponse<AuthResponse>.Failure(result.Message ?? string.Empty, System.Net.HttpStatusCode.TooManyRequests), statusCode: StatusCodes.Status429TooManyRequests),
                _ => Results.BadRequest(ApiResponse<AuthResponse>.Failure(result.Message ?? string.Empty))
            };
        })
        .Accepts<LoginCommand>("application/json")
        .DisableAntiforgery()
        .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status429TooManyRequests)
        .WithName("Login")
        .WithTags("Auth");
    }
}
