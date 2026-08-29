using FlowersApp.Catalog.Shared.Interfaces;

namespace FlowersApp.Catalog.Features.Stores.DeactivateStore
{
    public record DeactivateStoreCommand(Guid StoreId) : ICommand<bool>;

}
