using FlowersApp.Catalog.Shared.Interfaces;

namespace FlowersApp.Catalog.Features.Stores.GetCoverageGaps
{
    public record CoordinateDto(double Latitude, double Longitude);

    public record GetCoverageGapsQuery(List<CoordinateDto> CheckPoints) : IQuery<List<CoordinateDto>>;
}
