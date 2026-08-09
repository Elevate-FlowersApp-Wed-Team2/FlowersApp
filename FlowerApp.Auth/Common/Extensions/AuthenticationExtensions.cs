using FlowerApp.Auth.Common.Enums;
using FlowerApp.Auth.Infrastructure.Auth;
using FlowerApp.Auth.Infrastructure.Auth.Authorization;
using FlowerApp.Auth.Infrastructure.Auth.Authorization.Handlers;
using FlowerApp.Auth.Infrastructure.Auth.Authorization.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FlowerApp.Auth.Common.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration
                .GetSection("JwtSettings")
                .Get<JwtSettings>()!;

            services.Configure<JwtSettings>(
                configuration.GetSection("JwtSettings"));

            services.AddScoped<IJwtService, JwtService>();

            // JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key)),

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                };
            });

            // Authorization Policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthorizationPolicies.Admin,
                    policy => policy.RequireRole(
                        UserRole.Admin.ToString()));

                options.AddPolicy(
                    AuthorizationPolicies.Customer,
                    policy => policy.RequireRole(
                        UserRole.Customer.ToString()));

                options.AddPolicy(
                    AuthorizationPolicies.Driver,
                    policy => policy.RequireRole(
                        UserRole.Driver.ToString()));

                options.AddPolicy(
                    AuthorizationPolicies.ApprovedDriver,
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.AddRequirements(
                            new ApprovedDriverRequirement());
                    });
            });

            // Approved Driver Authorization Handler
            services.AddScoped<IAuthorizationHandler, ApprovedDriverHandler>();

            return services;
        }
    }
}