using FlowersApp.Cart.Shared.Interfaces;
using System.Net;

namespace FlowersApp.Cart.Shared.Services;
public class CatalogApiClient(HttpClient httpClient) : ICatalogApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<CatalogProductResponse?> GetProductAsync(Guid productId, CancellationToken cancellationToken)
    {
        var url = $"products/{productId}";

        using var response = await _httpClient.GetAsync(url, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CatalogProductResponse>(cancellationToken: cancellationToken);
    }
}