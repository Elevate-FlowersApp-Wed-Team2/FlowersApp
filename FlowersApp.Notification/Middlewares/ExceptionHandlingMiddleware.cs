using System.Net;
using FlowersApp.Notification.Shared.Response;
using FlowersApp.Shared.Exceptions;
using Newtonsoft.Json;

namespace FlowersApp.Notification.Middlewares;

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

            var errors = vex.Failures.Select(f => f.ErrorMessage).ToList();
            var apiResponse = ApiResponse<object>.Failure(errors, HttpStatusCode.BadRequest, "Validation failed");

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var message = _environment.IsDevelopment() ? ex.Message : "Something went wrong!";
            var apiResponse = ApiResponse<object>.Failure(message, HttpStatusCode.InternalServerError);
            
            await context.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse));
        }
    }
}
