using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Domain.Enum;
using FlowersApp.Catalog.Infrastructure.Persistence;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;

namespace FlowersApp.Catalog.Features.Stores.CreateStore
{
    public class CreateStoreHandler : ICommandHandler<CreateStoreCommand, Guid>
    {
        private readonly Repository<Store> _storeRepository;

        public CreateStoreHandler(Repository<Store> storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<RequestResult<Guid>> Handle(CreateStoreCommand cmd, CancellationToken ct)
        {
            var store = new Store
            {
                Name = cmd.Name,
                Address = cmd.Address,
                Latitude = cmd.Latitude,
                Longitude = cmd.Longitude,
                Status = StoreStatus.Active
            };

            // Repository.Add sets Id, CreatedAt, CreatedBy internally — store.Id is
            // populated after this call, safe to read below.
            _storeRepository.Add(store);
            await _storeRepository.SaveChangeAsync(ct);

            return RequestResult<Guid>.succeeded(store.Id, ResultCode.StoreCreated);
        }
    }
}
