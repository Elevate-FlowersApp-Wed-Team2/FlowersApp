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
        app.MapPost(Endpoints.Auth.DriverLogin, async (
            [FromBody] DriverLoginCommand request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request, cancellationToken);
            return result.Code switch
            {
                ResultCode.LoginSuccessful => Results.Ok(
                    ApiResponse<AuthResponse>.Success(result.Result, HttpStatusCode.OK, result.Message)),

                ResultCode.DriverAccountNotApproved or ResultCode.DriverApplicationRejected => Results.Json(
                    new ApiResponse<AuthResponse>(
                        false,
                        result.Result,
                        new List<string> { result.Message ?? string.Empty },
                        HttpStatusCode.Forbidden,
                        result.Message ?? string.Empty),
                    statusCode: StatusCodes.Status403Forbidden),

                ResultCode.TooManyFailedAttempts => Results.Json(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty, HttpStatusCode.TooManyRequests),
                    statusCode: StatusCodes.Status429TooManyRequests),

                ResultCode.InvalidCredentials => Results.Json(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty, HttpStatusCode.Unauthorized),
                    statusCode: StatusCodes.Status401Unauthorized),

                _ => Results.Json(
                    ApiResponse<object>.Failure(result.Message ?? string.Empty, HttpStatusCode.Unauthorized),
                    statusCode: StatusCodes.Status401Unauthorized)
            };
        })
        .AllowAnonymous()
        .Accepts<DriverLoginCommand>("application/json")
        .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<AuthResponse>>(StatusCodes.Status403Forbidden)
        .Produces<ApiResponse<object>>(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<object>>(StatusCodes.Status429TooManyRequests)
        .WithName("DriverLogin")
        .WithTags("Auth");
    }
}
