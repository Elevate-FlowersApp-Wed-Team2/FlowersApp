using DotNetEnv;
using FlowerApp.Auth.Domain;
using FlowerApp.Auth.Domain.Interfaces;
using FlowerApp.Auth.Features.Account.ChangePassword;
using FlowerApp.Auth.Infrastructure.Email;
using FlowerApp.Auth.Infrastructure.Persistence;
using FlowerApp.Auth.Infrastructure.Sessions;
using FlowersApp.Shared.Redis;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AuthDb")));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();
//register redis 
var redisConnection = builder.Configuration["REDIS_CONNECTION_STRING"]
                    ?? Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");

if (string.IsNullOrEmpty(redisConnection))
    throw new InvalidOperationException("REDIS_CONNECTION_STRING is not set. Check your .env file.");
builder.Services.AddRedisCache(redisConnection);

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// Email service configuration
builder.Services.Configure<EmailSettings>(options =>
{
    builder.Configuration.GetSection("Email").Bind(options);
    options.ApiKey = builder.Configuration["SENDGRID_API_KEY"]
        ?? Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
        ?? throw new InvalidOperationException("SENDGRID_API_KEY is not set.");
});
builder.Services.AddScoped<IEmailSender, SendGridEmailService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.MapChangePasswordEndpoint();
app.Run();