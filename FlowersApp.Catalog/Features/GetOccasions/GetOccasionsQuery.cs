using FlowersApp.Catalog.Shared.Interfaces;

namespace FlowersApp.Catalog.Features.GetOccasions
{
    public record GetOccasionsQuery : IQuery<List<OccasionResponse>>;
}
