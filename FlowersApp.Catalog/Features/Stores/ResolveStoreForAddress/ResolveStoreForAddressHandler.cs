using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Domain.Enum;
using FlowersApp.Catalog.Infrastructure.Persistence;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace FlowersApp.Catalog.Features.Stores.ResolveStoreForAddress
{
    public class ResolveStoreForAddressHandler : IQueryHandler<ResolveStoreForAddressQuery, Guid?>
    {
        private static readonly GeometryFactory GeometryFactory =
            NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        private readonly Repository<CoverageArea> _coverageAreaRepository;
        private readonly Repository<CoverageCity> _coverageCityRepository;
        private readonly Repository<AddressStoreAssignment> _assignmentRepository;

        public ResolveStoreForAddressHandler(
            Repository<CoverageArea> coverageAreaRepository,
            Repository<CoverageCity> coverageCityRepository,
            Repository<AddressStoreAssignment> assignmentRepository)
        {
            _coverageAreaRepository = coverageAreaRepository;
            _coverageCityRepository = coverageCityRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<RequestResult<Guid?>> Handle(ResolveStoreForAddressQuery q, CancellationToken ct)
        {
            var point = GeometryFactory.CreatePoint(new Coordinate(q.Longitude, q.Latitude));

            Guid? storeId = await _coverageAreaRepository
                .Get(c => c.IsActive && c.Store.Status == StoreStatus.Active && c.Geometry != null)
                .Where(c => c.Geometry!.Contains(point))
                .Select(c => (Guid?)c.StoreId)
                .FirstOrDefaultAsync(ct);

            if (storeId is null && !string.IsNullOrWhiteSpace(q.City))
            {
                storeId = await _coverageCityRepository
                    .Get(cc => cc.CoverageArea.IsActive && cc.CoverageArea.Store.Status == StoreStatus.Active)
                    .Where(cc => cc.CityName.ToLower() == q.City.ToLower())
                    .Select(cc => (Guid?)cc.CoverageArea.StoreId)
                    .FirstOrDefaultAsync(ct);
            }

            var existing = await _assignmentRepository
                .Get(a => a.AddressId == q.AddressId)
                .FirstOrDefaultAsync(ct);

            if (existing is null)
            {
                _assignmentRepository.Add(new AddressStoreAssignment
                {
                    AddressId = q.AddressId,
                    Latitude = q.Latitude,
                    Longitude = q.Longitude,
                    City = q.City,
                    StoreId = storeId,
                    IsUnresolved = storeId is null,
                    ResolvedAt = DateTime.UtcNow
                });
            }
            else
            {
                existing.Latitude = q.Latitude;
                existing.Longitude = q.Longitude;
                existing.City = q.City;
                existing.StoreId = storeId;
                existing.IsUnresolved = storeId is null;
                existing.ResolvedAt = DateTime.UtcNow;

                _assignmentRepository.SaveInclude(existing,
                    nameof(AddressStoreAssignment.Latitude),
                    nameof(AddressStoreAssignment.Longitude),
                    nameof(AddressStoreAssignment.City),
                    nameof(AddressStoreAssignment.StoreId),
                    nameof(AddressStoreAssignment.IsUnresolved),
                    nameof(AddressStoreAssignment.ResolvedAt));
            }

            await _assignmentRepository.SaveChangeAsync(ct);

            return storeId is null
                ? RequestResult<Guid?>.Failure(ResultCode.NoCoveringStoreFound)
                : RequestResult<Guid?>.succeeded(storeId, ResultCode.StoreUpdated);
        }
    }
}
