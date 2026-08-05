using FlowersApp.Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Extensions;

public static class InfrastructureDependanciesRegister
{
    public static IServiceCollection AddInfrastructureDependancies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        return services;
    }
}
