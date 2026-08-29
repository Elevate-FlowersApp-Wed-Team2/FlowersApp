using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Infrastructure.Persistence;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Shared.Extensions;
using FlowersApp.Shared.Interfaces;
using FlowersApp.Shared.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Catalog.Extensions;

public static class InfrastructureDependanciesRegister
{
    public static IServiceCollection AddInfrastructureDependancies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(Repository<>));

        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),sql => sql.UseNetTopologySuite());
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<UnitOfWork>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
}
