using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Catalog.Features.GetOccasions
{
    public class GetOccasionsHandler : IQueryHandler<GetOccasionsQuery, List<OccasionResponse>>
    {
        private readonly Repository<Occasion> _occasions;

        public GetOccasionsHandler(Repository<Occasion> occasions)
        {
            _occasions = occasions;
        }

        public async Task<RequestResult<List<OccasionResponse>>> Handle(
            GetOccasionsQuery request, CancellationToken cancellationToken)
        {
            var result = await _occasions
                .Get(o => o.IsActive)
                .OrderBy(o => o.SortOrder)
                .Select(o => new OccasionResponse(o.Id, o.Name, o.ImageUrl))
                .ToListAsync(cancellationToken);

            
            return RequestResult<List<OccasionResponse>>.succeeded(result, ResultCode.OccasionsRetrieved);
        }
    }
}
