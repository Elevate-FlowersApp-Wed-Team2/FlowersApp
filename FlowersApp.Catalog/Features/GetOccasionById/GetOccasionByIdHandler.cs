using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Extensions;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Response;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using FloweryApp.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Catalog.Features.GetOccasionById
{
    public class GetOccasionByIdHandler : IQueryHandler<GetOccasionByIdQuery, OccasionDetailsResponse>
    {
        private readonly Repository<Occasion> _occasions;

        public GetOccasionByIdHandler(Repository<Occasion> occasions)
        {
            _occasions = occasions;
        }

        public async Task<RequestResult<OccasionDetailsResponse>> Handle(
            GetOccasionByIdQuery request, CancellationToken cancellationToken)
        {
            var occasion = await _occasions
                .Get(o => o.Id == request.OccasionId)
                .FirstOrDefaultAsync(cancellationToken);

            if (occasion is null)
                return RequestResult<OccasionDetailsResponse>.Failure(ResultCode.OccasionNotFound);

            if (!occasion.IsActive)
                return RequestResult<OccasionDetailsResponse>.Failure(ResultCode.OccasionArchived);

            var response = new OccasionDetailsResponse(occasion.Id, occasion.Name, occasion.ImageUrl);
            return RequestResult<OccasionDetailsResponse>.succeeded(response, ResultCode.OccasionRetrieved);
        }
    }


}
