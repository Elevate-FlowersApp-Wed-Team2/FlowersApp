using FlowersApp.Auth.Infrastructure.Authentication;
using FlowersApp.Auth.Infrastructure.Authorization;
using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FlowersApp.Auth.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(options =>
        {
            configuration.GetSection(JwtOptions.SectionName).Bind(options);
            options.Key = configuration["Jwt:Key"]
                ?? configuration["JWT_KEY"]
                ?? Environment.GetEnvironmentVariable("JWT_KEY")
                ?? options.Key;
        });

        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
            ?? new JwtOptions();

        jwtOptions.Key = configuration["Jwt:Key"]
            ?? configuration["JWT_KEY"]
            ?? Environment.GetEnvironmentVariable("JWT_KEY")
            ?? jwtOptions.Key;

        if (string.IsNullOrWhiteSpace(jwtOptions.Key))
            throw new InvalidOperationException(
                "Jwt:Key is not set. Set Jwt:Key in configuration or JWT_KEY environment variable.");

        services.AddScoped<ITokenService, JwtTokenService>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = System.Security.Claims.ClaimTypes.Role,
                    NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.Admin, policy =>
                policy.RequireRole(UserRoles.Admin));

            options.AddPolicy(AuthorizationPolicies.Customer, policy =>
                policy.RequireRole(UserRoles.Customer));

            options.AddPolicy(AuthorizationPolicies.Driver, policy =>
                policy.RequireRole(UserRoles.Driver));

            options.AddPolicy(AuthorizationPolicies.ApprovedDriver, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new ApprovedDriverRequirement());
            });
        });

        services.AddScoped<IAuthorizationHandler, ApprovedDriverHandler>();

        return services;
    }
}
