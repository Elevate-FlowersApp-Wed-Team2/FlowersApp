using FlowersApp.Notification.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Notification.Extensions
{
    public static class InfrastructureDependanciesRegister
    {
        public static IServiceCollection AddInfrastructureDependancies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NotificationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            return services;
        }
    }
}
