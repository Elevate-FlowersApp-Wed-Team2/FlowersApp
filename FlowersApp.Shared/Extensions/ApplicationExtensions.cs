using FlowersApp.Auth.Shared.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FlowersApp.Shared.Interfaces;

namespace FlowersApp.Shared.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddDependancies<TServiceMarker>(this IServiceCollection services)
    {
        //Add Behaviour 
        services.AddTransient(typeof(IPipelineBehavior<,>),
                            typeof(ValidationBehaviour<,>));
        //Add Mediatr
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(TServiceMarker).Assembly);
        });

        //Add Fluent Validation
        services.AddValidatorsFromAssembly(typeof(TServiceMarker).Assembly);
        services.AddScoped<ICurrentUserService, ICurrentUserService>();
        return services;
    }
}

