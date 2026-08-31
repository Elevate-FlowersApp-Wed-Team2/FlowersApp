using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Domain.Enum;
using FlowersApp.Catalog.Features.Stores.ResolveStoreForAddress;
using FlowersApp.Catalog.Infrastructure.Persistence;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Catalog.Features.Stores.DeactivateStore
{
    //public record ReassignAddressesBatchCommand(List<AddressStoreAssignment> Assignments) : ICommand<bool>;
    //public class DeactivateStoreHandler : ICommandHandler<DeactivateStoreCommand, bool>
    //{
    //    private readonly CatalogDbContext _db;
    //    private readonly ISender _mediator;

    //    public DeactivateStoreHandler(CatalogDbContext db, ISender mediator)
    //    {
    //        _db = db;
    //        _mediator = mediator;
    //    }

    //    public async Task<RequestResult<bool>> Handle(DeactivateStoreCommand cmd, CancellationToken ct)
    //    {
    //        var store = await _db.Stores.FirstOrDefaultAsync(s => s.Id == cmd.StoreId, ct);
    //        if (store is null)
    //            return RequestResult<bool>.Failure(ResultCode.StoreNotFound);

    //        // 1. Update Store Status
    //        store.Status = StoreStatus.Inactive;
    //        store.UpdatedAt = DateTime.UtcNow;

    //        // 2. High Performance Batch Update (EF Core 7+)
    //        await _db.CoverageAreas
    //            .Where(c => c.StoreId == store.Id)
    //            .ExecuteUpdateAsync(s => s.SetProperty(c => c.IsActive, false), ct);

    //        // 3. Fetch affected assignments in memory
    //        var affected = await _db.AddressStoreAssignments
    //            .AsNoTracking()
    //            .Where(a => a.StoreId == store.Id)
    //            .ToListAsync(ct);

    //        await _db.SaveChangesAsync(ct);

    //        // 4. Batch Re-resolution (Or Background Processing)
    //        if (affected.Count > 0)
    //        {
    //            await _mediator.Send(new ReassignAddressesBatchCommand(affected), ct);
    //        }

    //        return RequestResult<bool>.succeeded(true, ResultCode.StoreDeactivated);
    //    }
    //}
    public class DeactivateStoreHandler : ICommandHandler<DeactivateStoreCommand, bool>
    {
        private readonly Repository<Store> _storeRepository;
        private readonly Repository<CoverageArea> _coverageAreaRepository;
        private readonly Repository<AddressStoreAssignment> _assignmentRepository;
        private readonly IMediator _mediator;

        public DeactivateStoreHandler(
            Repository<Store> storeRepository,
            Repository<CoverageArea> coverageAreaRepository,
            Repository<AddressStoreAssignment> assignmentRepository,
            IMediator mediator)
        {
            _storeRepository = storeRepository;
            _coverageAreaRepository = coverageAreaRepository;
            _assignmentRepository = assignmentRepository;
            _mediator = mediator;
        }

        public async Task<RequestResult<bool>> Handle(DeactivateStoreCommand cmd, CancellationToken ct)
        {
            var store = await _storeRepository.Get(s => s.Id == cmd.StoreId).FirstOrDefaultAsync(ct);
            if (store is null)
                return RequestResult<bool>.Failure(ResultCode.StoreNotFound);

            store.Status = StoreStatus.Inactive;
            store.UpdatedAt = DateTime.UtcNow;
            _storeRepository.SaveInclude(store, nameof(Store.Status), nameof(Store.UpdatedAt));

            var areas = await _coverageAreaRepository.Get(c => c.StoreId == store.Id).ToListAsync(ct);
            foreach (var area in areas)
            {
                area.IsActive = false;
                _coverageAreaRepository.SaveInclude(area, nameof(CoverageArea.IsActive));
            }

            var affected = await _assignmentRepository.Get(a => a.StoreId == store.Id).ToListAsync(ct);

            await _storeRepository.SaveChangeAsync(ct);

            foreach (var assignment in affected)
            {
                await _mediator.Send(new ResolveStoreForAddressQuery(
                    assignment.AddressId, assignment.Latitude, assignment.Longitude, assignment.City), ct);
            }

            return RequestResult<bool>.succeeded(true, ResultCode.StoreDeactivated);
        }
    }
}
