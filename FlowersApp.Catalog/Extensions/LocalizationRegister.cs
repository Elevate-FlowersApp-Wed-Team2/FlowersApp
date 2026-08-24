using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Linq;

namespace FlowersApp.Catalog.Extensions;

public static class LocalizationRegister
{
    public static IServiceCollection AddAppLocalization(this IServiceCollection services)
    {
        // Use default localization resource lookup (resx placed under namespace folders)
        services.AddLocalization();

        var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("ar") };
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
            options.SupportedCultures = supportedCultures.ToList();
            options.SupportedUICultures = supportedCultures.ToList();

            // This reads Accept-Language automatically
            options.RequestCultureProviders = new List<IRequestCultureProvider>
               {
                   new AcceptLanguageHeaderRequestCultureProvider()
               };
        });
        return services;
    }
}
