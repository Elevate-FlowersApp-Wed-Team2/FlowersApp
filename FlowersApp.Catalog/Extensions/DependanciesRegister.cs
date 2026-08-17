namespace FlowersApp.Catalog.Extensions;

public static class DependanciesRegister
{
    public static IServiceCollection AddDependencies(this IServiceCollection services ,IConfiguration configuration)
    {
        services.AddInfrastructureDependancies(configuration);
        services.AddApplicationDependancies(configuration);
        services.AddAppLocalization();
        return services;
    }
}
