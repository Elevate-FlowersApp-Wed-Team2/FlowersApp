using Microsoft.AspNetCore.Localization;

namespace FlowersApp.Auth.Extensions;

public static class LocalizationRegister
{
    public static IServiceCollection AddLocalization(this IServiceCollection services)
    {
        // Program.cs
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        var supportedCultures = new[] { "en", "ar"};
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            // This is what reads Accept-Language automatically
            options.RequestCultureProviders = new List<IRequestCultureProvider>
               {
                   new AcceptLanguageHeaderRequestCultureProvider()
               };
        });
        return services;
    }
}
