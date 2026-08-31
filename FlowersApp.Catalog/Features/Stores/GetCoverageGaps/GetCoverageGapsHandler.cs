using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Domain.Enum;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace FlowersApp.Catalog.Features.Stores.GetCoverageGaps
{
    public class GetCoverageGapsHandler : IQueryHandler<GetCoverageGapsQuery, List<CoordinateDto>>
    {
        private static readonly GeometryFactory GeometryFactory =
            NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        private readonly Repository<CoverageArea> _coverageAreaRepository;

        public GetCoverageGapsHandler(Repository<CoverageArea> coverageAreaRepository)
        {
            _coverageAreaRepository = coverageAreaRepository;
        }

        public async Task<RequestResult<List<CoordinateDto>>> Handle(GetCoverageGapsQuery q, CancellationToken ct)
        {
            var gaps = new List<CoordinateDto>();

            foreach (var point in q.CheckPoints)
            {
                var geoPoint = GeometryFactory.CreatePoint(new Coordinate(point.Longitude, point.Latitude));

                var covered = await _coverageAreaRepository
                    .Get(c => c.IsActive && c.Store.Status == StoreStatus.Active && c.Geometry != null)
                    .AnyAsync(c => c.Geometry!.Contains(geoPoint), ct);

                if (!covered)
                    gaps.Add(point);
            }

            return RequestResult<List<CoordinateDto>>.succeeded(gaps,
                gaps.Any() ? ResultCode.StoreCoverageGapsFound : ResultCode.StoreUpdated);
        }
    }
}
