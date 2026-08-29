using FlowersApp.Catalog.Shared.Interfaces;

namespace FlowersApp.Catalog.Features.Stores.CreateStore
{
    public record CreateStoreCommand(
     string Name,
     string Address,
     double Latitude,
     double Longitude
    ) : ICommand<Guid>;
}
