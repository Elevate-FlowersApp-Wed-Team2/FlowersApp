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

        // Redis connection string
        var redisConnection =
            builder.Configuration["REDIS_CONNECTION_STRING"]
            ?? Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(redisConnection))
        {
            throw new InvalidOperationException(
                "REDIS_CONNECTION_STRING is not set. Set it in appsettings, environment variables, or a local .env file.");
        }

        builder.Services.AddRedisCache(redisConnection);

        // Email service configuration
        builder.Services.Configure<EmailSettings>(options =>
        {
            builder.Configuration.GetSection("Email").Bind(options);

            options.ApiKey = builder.Configuration["SENDGRID_API_KEY"]
                ?? Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
                ?? throw new InvalidOperationException("SENDGRID_API_KEY is not set.");
        });
        

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.OperationFilter<AcceptLanguageHeaderOperationFilter>();

            options.AddSecurityDefinition("Bearer",
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

            options.AddSecurityRequirement(
                new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        });

        var app = builder.Build();

        var opts = app.Services
            .GetRequiredService<IOptions<RequestLocalizationOptions>>()
            .Value;

        app.UseRequestLocalization(opts);

        // Show the real exception while debugging
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

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

        // app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void LoadDotEnv()
    {
        var options = new LoadOptions(
            setEnvVars: true,
            clobberExistingVars: false);

        var candidates = new[]
        {
            Path.Combine(Directory.GetCurrentDirectory(), ".env"),
            Path.Combine(AppContext.BaseDirectory, ".env"),

            // bin/Debug/net8.0 -> project directory
            Path.GetFullPath(
                Path.Combine(
                    AppContext.BaseDirectory,
                    "..",
                    "..",
                    "..",
                    ".env")),

            // project -> solution root
            Path.GetFullPath(
                Path.Combine(
                    AppContext.BaseDirectory,
                    "..",
                    "..",
                    "..",
                    "..",
                    ".env")),
        };

        foreach (var path in candidates.Distinct(
            StringComparer.OrdinalIgnoreCase))
        {
            if (!File.Exists(path))
                continue;

            Env.Load(path, options);
            return;
        }
    }
}