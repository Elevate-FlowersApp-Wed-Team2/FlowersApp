namespace FlowersApp.Notification.Extensions
{
    public static class DependanciesRegister
    {
        public static IServiceCollection AddDependancies(this IServiceCollection services,IConfiguration configuration) {

            services.AddInfrastructureDependancies(configuration);
            return services;
        }
    }
}
