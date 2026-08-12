using DotNetEnv;
using FlowerApp.Auth.Domain.Interfaces;
using FlowerApp.Auth.Infrastructure.Email;
using FlowersApp.Auth.Extensions;
using FlowersApp.Auth.Middlewares;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Services;
using FlowersApp.Shared.Redis;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using SendGrid;
using System.Globalization;

namespace FlowersApp.Auth;

public class Program
{
    public static void Main(string[] args)
    {
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
        if (File.Exists(envPath))
        {
            Env.Load(envPath, new LoadOptions(setEnvVars: true, clobberExistingVars: false));
        }
        else
        {
            Env.Load();
        }
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());

        builder.Services.AddControllers();

        builder.Services.AddDependencies(builder.Configuration);


        /*
         * IMPORTANT:
         * Service registrations must happen before calling builder.Build().
         *
         * During the previous merge conflict resolution, Redis registration
         * was accidentally placed after builder.Build(), which is incorrect.
         *
         * After the application is built, the dependency injection container
         * is already created and should not be modified.
         *
         * Only middleware configuration (app.Use...) and endpoint mapping
         * (app.Map...) should be added after builder.Build().
         */
        //redis connection string
        var redisConnection =
            builder.Configuration["REDIS_CONNECTION_STRING"]
            ?? Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(redisConnection))
        {
            throw new InvalidOperationException(
                "REDIS_CONNECTION_STRING is not set. Check your .env file.");
        }

        builder.Services.AddRedisCache(redisConnection);
         //sendgrid api key
        // Email service configuration
        builder.Services.Configure<EmailSettings>(options =>
        {
            builder.Configuration.GetSection("Email").Bind(options);
            options.ApiKey = builder.Configuration["SENDGRID_API_KEY"]
                ?? Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
                ?? throw new InvalidOperationException("SENDGRID_API_KEY is not set.");
        });
        


        //builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.OperationFilter<AcceptLanguageHeaderOperationFilter>();
        });


        var app = builder.Build();
        var opts = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
        app.UseRequestLocalization(opts);
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = check => check.Name == "self"
        });


        /*
         * Middleware should be configured after builder.Build().
         *
         * UseRequestLocalization is part of the HTTP request pipeline,
         * therefore it belongs here and not before building the application.
         */

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


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

       // app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}