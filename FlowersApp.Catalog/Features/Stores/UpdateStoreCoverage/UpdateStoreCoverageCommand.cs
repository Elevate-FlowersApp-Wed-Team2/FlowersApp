using FlowersApp.Catalog.Domain.Enum;
using FlowersApp.Catalog.Shared.Interfaces;

namespace FlowersApp.Catalog.Features.Stores.UpdateStoreCoverage
{
    public record CoveragePointDto(double Latitude, double Longitude);

    public record UpdateStoreCoverageCommand(
     Guid StoreId,
     CoverageType Type,
     List<CoveragePointDto>? PolygonPoints,   // required if Type == Polygon
     double? CenterLatitude,                  // required if Type == Radius
     double? CenterLongitude,
     double? RadiusMeters,
     List<string>? Cities                     // required if Type == CityList
 ) : ICommand<Guid>;
}
