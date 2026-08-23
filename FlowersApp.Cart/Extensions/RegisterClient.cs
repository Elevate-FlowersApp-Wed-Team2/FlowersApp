using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Services;
using Polly;
using Microsoft.Extensions.Http; // AddPolicyHandler extension
using Polly.Extensions.Http;


namespace FlowersApp.Cart.Extensions;

public static class RegisterClient
{
    public static IServiceCollection RegisterClients(this IServiceCollection services)
    {
        services.AddHttpClient<ICatalogApiClient, CatalogApiClient>(client =>
        {
            // "flowersapp.catalog" resolved via Docker Compose service name / service discovery
            client.BaseAddress = new Uri("http://flowersapp.catalog/api/v1");
            client.Timeout = TimeSpan.FromSeconds(5);
        })
         .AddPolicyHandler(GetRetryPolicy())
         .AddPolicyHandler(GetCircuitBreakerPolicy());
        return services;
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(200 * retryAttempt));

    static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}
