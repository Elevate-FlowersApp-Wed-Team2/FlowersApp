using FlowersApp.Shared.Extensions;

namespace FlowersApp.Notification.Extensions
{
    public static class DependanciesRegister
    {
        public static IServiceCollection AddDependancies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDependancies<Program>();
            services.AddInfrastructureDependancies(configuration);
            services.AddFirebase(configuration);
            return services;
        }
    }
}
