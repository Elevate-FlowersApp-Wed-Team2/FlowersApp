using FlowersApp.Notification.Extensions;
using FlowersApp.Notification.Middlewares;
using FlowersApp.Notification.Shared.Interfaces;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FlowersApp.Notification;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDependancies(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = check => check.Name == "self"
        });

        var globalGroup = app.MapGroup("");

        var endpointDefinitions = typeof(Program).Assembly
            .GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>();

        foreach (var endpoint in endpointDefinitions)
        {
            endpoint.MapEndpoint(globalGroup);
        }

        app.ApplyDatabaseMigrations(app.Logger);

        app.Run();
    }
}
