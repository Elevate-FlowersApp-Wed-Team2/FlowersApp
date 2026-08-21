using FlowersApp.Catalog.Domain.Enum;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using FloweryApp.Api.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FloweryApp.Api.Features.Home.GetHomeSections;

public sealed record GetHomeSectionsQuery(string Language)
    : IQuery<IReadOnlyList<HomeSectionResponse>>;

public record HomeSectionResponse(Guid Id,string Type ,string Title, int Index, bool IsActive
    ,Guid? OccasionId , Guid? CategoryId);

public class GetHomeSectionsQueryHandler(Repository<Section> repository) : IQueryHandler<GetHomeSectionsQuery, IReadOnlyList<HomeSectionResponse>>
{
    private readonly Repository<Section> _repository = repository;

    public async Task<RequestResult<IReadOnlyList<HomeSectionResponse>>> Handle(GetHomeSectionsQuery request, CancellationToken cancellationToken)
    {
        if(!Enum.TryParse<Languages>(request.Language,true ,out var languages))
            return RequestResult<IReadOnlyList<HomeSectionResponse>>.Failure(ResultCode.NotSupportedLanguage);
        var result = languages switch
        {
            Languages.en or Languages.English => await _repository.Get(s => s.IsActive)
                                      .OrderBy(s => s.Index)
                                      .Select(s => new HomeSectionResponse
                                      (
                                          s.Id, s.Type.ToString(), s.Title, s.Index, s.IsActive,
                                          s.OccasionId, s.CategoryId

                                      )).ToListAsync(cancellationToken),
            Languages.ar or Languages.Arabic => await _repository.Get(s => s.IsActive)
                                     .OrderBy(s => s.Index)
                                      .Select(s => new HomeSectionResponse
                                      (
                                          s.Id, s.Type.ToString(), s.ArabicTitle, s.Index, s.IsActive,
                                          s.OccasionId, s.CategoryId

                                      )).ToListAsync(cancellationToken),
            _ => new List<HomeSectionResponse>()
        };
        return RequestResult<IReadOnlyList<HomeSectionResponse>>.succeeded(result, ResultCode.SectionRetrieved);
    }
}
