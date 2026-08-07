using FlowersApp.Auth.Shared.Exceptions;
using FlowersApp.Auth.Shared.Response;
using Newtonsoft.Json;
using System.Net;

namespace FlowersApp.Auth.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException vex)
        {
            _logger.LogError(vex, "A validation error occurred: {Message}", vex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            var response = JsonConvert.SerializeObject(vex.Failure); ;
            await context.Response.WriteAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            if (_environment.IsDevelopment())
            {
                await context.Response.WriteAsJsonAsync(ApiResponse<object>.Failure(ex.Message, HttpStatusCode.InternalServerError));
                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var apiResponse = ApiResponse<object>.Failure("Somthing went wrong!");
            var response = JsonConvert.SerializeObject(apiResponse);
            await context.Response.WriteAsync(response);
        }
    }
}