using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Domain.Enum;
using FlowersApp.Catalog.Infrastructure.Persistence;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Catalog.Features.Stores.GetCoverageOverlaps
{
    public class GetCoverageOverlapsHandler : IQueryHandler<GetCoverageOverlapsQuery, List<OverlapPairDto>>
    {
        private readonly Repository<CoverageArea> _coverageAreaRepository;
        private readonly Repository<CoverageCity> _coverageCityRepository;

        public GetCoverageOverlapsHandler(
            Repository<CoverageArea> coverageAreaRepository,
            Repository<CoverageCity> coverageCityRepository)
        {
            _coverageAreaRepository = coverageAreaRepository;
            _coverageCityRepository = coverageCityRepository;
        }

        public async Task<RequestResult<List<OverlapPairDto>>> Handle(GetCoverageOverlapsQuery q, CancellationToken ct)
        {
            var areas = await _coverageAreaRepository
                .Get(c => c.IsActive && c.Geometry != null && c.Store.Status == StoreStatus.Active)
                .Include(c => c.Store)
                .ToListAsync(ct);

            var overlaps = new List<OverlapPairDto>();
            for (int i = 0; i < areas.Count; i++)
                for (int j = i + 1; j < areas.Count; j++)
                    if (areas[i].StoreId != areas[j].StoreId && areas[i].Geometry!.Intersects(areas[j].Geometry!))
                        overlaps.Add(new OverlapPairDto(
                            areas[i].StoreId, areas[i].Store.Name,
                            areas[j].StoreId, areas[j].Store.Name));

            var cityOverlaps = await _coverageCityRepository
                .Get(c => c.CoverageArea.IsActive)
                .Include(c => c.CoverageArea).ThenInclude(ca => ca.Store)
                .GroupBy(c => c.CityName)
                .Where(g => g.Select(x => x.CoverageArea.StoreId).Distinct().Count() > 1)
                .Select(g => g.Key)
                .ToListAsync(ct);

            return RequestResult<List<OverlapPairDto>>.succeeded(overlaps,
                overlaps.Any() || cityOverlaps.Any()
                    ? ResultCode.StoreCoverageOverlapsFound
                    : ResultCode.StoreUpdated);
        }
    }
}
