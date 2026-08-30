using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using System.Net;
using System.Text.Json;

namespace FlowersApp.Cart.Shared.Services;
public class CatalogApiClient(HttpClient httpClient ,ILogger<CatalogApiClient> logger) : ICatalogApiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<CatalogApiClient> _logger = logger;
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);
    public async Task<CatalogProductResponse?> GetProductAsync(Guid productId, CancellationToken cancellationToken)
    {
        _httpClient.BaseAddress = new Uri("http://flowersapp.catalog:8080/api/v1/catalog/");
        var url = $"products/{productId}";

        using var response = await _httpClient.GetAsync(url, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var raw = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogInformation("Catalog raw response: {Raw}", raw); // or just breakpoint here
        var wrapper = await response.Content.ReadFromJsonAsync<ApiResponse<CatalogProductResponse>>(_jsonOptions, cancellationToken);
        return wrapper?.Data;
    }
}