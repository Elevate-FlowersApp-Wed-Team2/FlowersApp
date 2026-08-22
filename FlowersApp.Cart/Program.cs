using FlowersApp.Cart.Extensions;
using FlowersApp.Cart.Middlewares;
using FlowersApp.Cart.Shared.Interfaces;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FlowersApp.Cart;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddHealthChecks()
               .AddCheck("self", () => HealthCheckResult.Healthy());
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDependencies(builder.Configuration);
        var app = builder.Build();

        // Configure the HTTP request pipeline.
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
        //app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
