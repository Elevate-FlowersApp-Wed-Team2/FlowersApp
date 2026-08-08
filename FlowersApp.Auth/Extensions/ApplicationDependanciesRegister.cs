using FlowersApp.Auth.Shared.Behaviours;
using FlowersApp.Auth.Shared.Services;
using FluentValidation;
using MediatR;

namespace FlowersApp.Auth.Extensions;

public static class ApplicationDependanciesRegister
{
    public static IServiceCollection AddApplicationDependancies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>),
                    typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LocalizationBehavior<,>));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationDependanciesRegister).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LocalizationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(typeof(ApplicationDependanciesRegister).Assembly);
        services.AddAppLocalization();
        return services;
    }
}
