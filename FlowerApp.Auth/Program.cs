using FlowerApp.Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using FlowersApp.Shared.Redis;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AuthDb")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//register redis 
var redisConnection = builder.Configuration["REDIS_CONNECTION_STRING"]
                    ?? Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");

if (string.IsNullOrEmpty(redisConnection))
    throw new InvalidOperationException("REDIS_CONNECTION_STRING is not set. Check your .env file.");

builder.Services.AddRedisCache(redisConnection);
var app = builder.Build();

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