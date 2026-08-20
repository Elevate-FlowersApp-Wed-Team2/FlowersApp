using FlowersApp.Catalog.Shared.Interfaces;

namespace FlowersApp.Catalog.Features.GetOccasionById
{
    public record GetOccasionByIdQuery(Guid OccasionId) : IQuery<OccasionDetailsResponse>;
}
