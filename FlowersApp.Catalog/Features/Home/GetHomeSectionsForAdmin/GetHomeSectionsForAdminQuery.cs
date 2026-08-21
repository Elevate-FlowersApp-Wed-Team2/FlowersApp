using FlowersApp.Catalog.Domain.Enum;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using FloweryApp.Api.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FloweryApp.Api.Features.Home.GetHomeSectionsForAdmin;

public sealed record GetHomeSectionsForAdminQuery()
    : IQuery<IReadOnlyList<GetHomeSectionsForAdminResponse>>;

public record GetHomeSectionsForAdminResponse(Guid Id,string Type ,string Title,string ArabicTitle , int Index, bool IsActive
    ,Guid? OccasionId , Guid? CategoryId);

public class GetHomeSectionsForAdminQueryHandler(Repository<Section> repository) 
    : IQueryHandler<GetHomeSectionsForAdminQuery, IReadOnlyList<GetHomeSectionsForAdminResponse>>
{
    private readonly Repository<Section> _repository = repository;

    public async Task<RequestResult<IReadOnlyList<GetHomeSectionsForAdminResponse>>> Handle(GetHomeSectionsForAdminQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.Get()
                                      .OrderBy(s => s.Index)
                                      .Select(s => new GetHomeSectionsForAdminResponse
                                      (
                                          s.Id, s.Type.ToString(), s.Title, s.ArabicTitle,s.Index, s.IsActive,
                                          s.OccasionId, s.CategoryId

                                      )).ToListAsync(cancellationToken);
        return RequestResult<IReadOnlyList<GetHomeSectionsForAdminResponse>>.succeeded(result, ResultCode.SectionRetrieved);
    }
}
