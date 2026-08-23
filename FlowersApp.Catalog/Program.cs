using FlowersApp.Catalog.Extensions;
using FlowersApp.Catalog.Infrastructure.Persistence;
using FlowersApp.Catalog.Middlewares;
using FlowersApp.Catalog.Shared.Interfaces;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace FlowersApp.Catalog;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHealthChecks()
               .AddCheck("self", () => HealthCheckResult.Healthy());
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAppLocalization();
        builder.Services.AddSwaggerGen(c =>
        {
            c.OperationFilter<AcceptLanguageHeaderOperationFilter>();
        });
        builder.Services.AddDependencies(builder.Configuration);
        var app = builder.Build();
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
        // Enable request localization using configured options
        var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
        app.UseRequestLocalization(locOptions);
        // Diagnostic: log embedded resource names so we can verify .resx compiled names
        try
        {
            var asm = typeof(Program).Assembly;
            var names = asm.GetManifestResourceNames();
            foreach (var n in names)
            {
                app.Logger.LogInformation("Embedded resource: {name}", n);
            }
        }
        catch (Exception ex)
        {
            app.Logger.LogWarning(ex, "Failed to enumerate embedded resources");
        }
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
