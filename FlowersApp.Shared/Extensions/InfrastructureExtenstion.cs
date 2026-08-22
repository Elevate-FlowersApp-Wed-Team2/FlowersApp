using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlowersApp.Shared.Extensions;

public static class InfrastructureExtenstion
{
    public static IServiceCollection AddInfrastructureDependancies<TDBContext>(this IServiceCollection services, IConfiguration configuration)
        where TDBContext : DbContext
    {
        services.AddDbContext<TDBContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }
}