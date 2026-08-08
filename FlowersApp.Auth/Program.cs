
using FlowersApp.Auth.Extensions;
using FlowersApp.Auth.Middlewares;
using FlowersApp.Auth.Shared.Interfaces;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;
using DotNetEnv;
using FlowersApp.Shared.Redis;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace FlowersApp.Auth;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHealthChecks()
               .AddCheck("self", () => HealthCheckResult.Healthy());
        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddDependencies(builder.Configuration);
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        var opts = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
        app.UseRequestLocalization(opts);
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = check => check.Name == "self"
        });

        //register redis 
        var redisConnection = builder.Configuration["REDIS_CONNECTION_STRING"]
                            ?? Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");

        if (string.IsNullOrEmpty(redisConnection))
            throw new InvalidOperationException("REDIS_CONNECTION_STRING is not set. Check your .env file.");

        builder.Services.AddRedisCache(redisConnection);

        app.UseRequestLocalization();
        // Configure the HTTP request pipeline.
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
        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
