
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;
using DotNetEnv;
using FlowersApp.Shared.Redis;
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
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
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

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
