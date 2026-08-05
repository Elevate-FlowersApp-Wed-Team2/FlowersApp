
namespace FlowersApp.Auth.Extensions;

public static class ApplicationDependanciesRegister
{
    public static IServiceCollection AddApplicationDependancies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationDependanciesRegister).Assembly)
        });
        return services;
    }
}
