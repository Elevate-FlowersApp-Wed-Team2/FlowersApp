namespace FlowersApp.Auth.Extensions;

public static class DependanciesRegister
{
    public static IServiceCollection AddDependencies(this IServiceCollection services ,IConfiguration configuration)
    {
        services.AddInfrastructureDependancies(configuration);
        return services;
    }
}
