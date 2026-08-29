using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Domain.Enum;
using FlowersApp.Catalog.Infrastructure.Persistence;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace FlowersApp.Catalog.Features.Stores.UpdateStoreCoverage
{
    public class UpdateStoreCoverageHandler : ICommandHandler<UpdateStoreCoverageCommand, Guid>
    {
        private static readonly GeometryFactory GeometryFactory =
            NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        private readonly Repository<Store> _storeRepository;
        private readonly Repository<CoverageArea> _coverageAreaRepository;

        public UpdateStoreCoverageHandler(
            Repository<Store> storeRepository,
            Repository<CoverageArea> coverageAreaRepository)
        {
            _storeRepository = storeRepository;
            _coverageAreaRepository = coverageAreaRepository;
        }

        public async Task<RequestResult<Guid>> Handle(UpdateStoreCoverageCommand cmd, CancellationToken ct)
        {
            var store = await _storeRepository
                .Get(s => s.Id == cmd.StoreId)
                .FirstOrDefaultAsync(ct);

            if (store is null)
                return RequestResult<Guid>.Failure(ResultCode.StoreNotFound);

            var coverage = new CoverageArea
            {
                StoreId = store.Id,
                Type = cmd.Type,
                IsActive = true
            };

            switch (cmd.Type)
            {
                case CoverageType.Polygon:
                    var coords = cmd.PolygonPoints!
                        .Select(p => new Coordinate(p.Longitude, p.Latitude))
                        .ToList();
                    if (!coords.First().Equals2D(coords.Last()))
                        coords.Add(coords.First());
                    coverage.Geometry = GeometryFactory.CreatePolygon(coords.ToArray());
                    break;

                case CoverageType.Radius:
                    var center = GeometryFactory.CreatePoint(
                        new Coordinate(cmd.CenterLongitude!.Value, cmd.CenterLatitude!.Value));
                    var degrees = cmd.RadiusMeters!.Value / 111_320.0;
                    coverage.Geometry = (Polygon)center.Buffer(degrees);
                    coverage.CenterLatitude = cmd.CenterLatitude;
                    coverage.CenterLongitude = cmd.CenterLongitude;
                    coverage.RadiusMeters = cmd.RadiusMeters;
                    break;

                case CoverageType.CityList:
                    
                    coverage.Cities = cmd.Cities!
                        .Select(c => new CoverageCity
                        {
                            Id = Guid.NewGuid(),
                            CreatedAt = DateTime.UtcNow,
                            CityName = c
                        })
                        .ToList();
                    break;
            }

            _coverageAreaRepository.Add(coverage);
            await _coverageAreaRepository.SaveChangeAsync(ct);

            return RequestResult<Guid>.succeeded(coverage.Id, ResultCode.CoverageUpdated);
        }
    }
}

