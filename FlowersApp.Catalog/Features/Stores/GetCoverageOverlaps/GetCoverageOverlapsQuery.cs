using FlowersApp.Catalog.Shared.Interfaces;

namespace FlowersApp.Catalog.Features.Stores.GetCoverageOverlaps
{
    public record GetCoverageOverlapsQuery : IQuery<List<OverlapPairDto>>;

}
