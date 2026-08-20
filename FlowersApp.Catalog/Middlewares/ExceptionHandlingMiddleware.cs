using FlowersApp.Shared.Exceptions;
using FlowersApp.Catalog.Shared.Response;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Net;
using FlowersApp.Catalog.Response;

namespace FlowersApp.Catalog.Middlewares;

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

    public async Task InvokeAsync(HttpContext context, IStringLocalizer<ErrorMessages> localizer)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException vex)
        {
            _logger.LogError(vex, "A validation error occurred: {Message}", vex.Message);

            var errors = vex.Failures.Select(f => LocalizeFailure(f, localizer)).ToList();
            var apiResponse = ApiResponse<object>.Failure(errors, HttpStatusCode.BadRequest, localizer["ValidationFailureMessage"]);

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse));
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


    private static string LocalizeFailure(ValidationFailure failure, IStringLocalizer<ErrorMessages> localizer)
    {
        var key = string.IsNullOrEmpty(failure.ErrorCode) ? "Validation_Required" : failure.ErrorCode;
        var localized = localizer[key, failure.PropertyName];
        return localized.ResourceNotFound ? failure.ErrorMessage : localized.Value;
    }
}