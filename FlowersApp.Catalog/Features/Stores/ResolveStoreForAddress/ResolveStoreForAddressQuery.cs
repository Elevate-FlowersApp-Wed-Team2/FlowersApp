using FlowersApp.Catalog.Shared.Interfaces;

namespace FlowersApp.Catalog.Features.Stores.ResolveStoreForAddress
{
    public record ResolveStoreForAddressQuery(
     Guid AddressId,
     double Latitude,
     double Longitude,
     string? City
    ) : IQuery<Guid?>;
}
