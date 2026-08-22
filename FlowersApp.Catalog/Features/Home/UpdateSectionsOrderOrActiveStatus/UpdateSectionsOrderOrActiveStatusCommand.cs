using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using FloweryApp.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Catalog.Features.Home.UpdateSectionsOrderOrActiveStatus;

public record UpdateSectionsOrderOrActiveStatusCommand
(List<UpdateSectionOrderOrActiveStatusRequest> Sections)
    : ICommand<List<UpdateSectionOrderOrActiveStatusResponse>>;
public record UpdateSectionOrderOrActiveStatusRequest
    (Guid Id, bool IsActive, int Index);

public record UpdateSectionOrderOrActiveStatusResponse(Guid Id, string Type, string Title, int Index, bool IsActive
    , Guid? OccasionId, Guid? CategoryId);

public class UpdateSectionsOrderOrActiveStatusCommandHandler(Repository<Section> repository)
    : ICommandHandler<UpdateSectionsOrderOrActiveStatusCommand, List<UpdateSectionOrderOrActiveStatusResponse>>
{
    private readonly Repository<Section> _repository = repository;

    public async Task<RequestResult<List<UpdateSectionOrderOrActiveStatusResponse>>> Handle(
        UpdateSectionsOrderOrActiveStatusCommand request, CancellationToken cancellationToken)
    {
        var sectionIds = request.Sections.Select(s => s.Id).ToList();

        var existingSections = await _repository.Get(s => sectionIds.Contains(s.Id))
            .Select(s => new
            {
                s.Id,
                s.CategoryId,
                s.Title,
                s.Type,
                s.OccasionId,
            })
            .ToListAsync(cancellationToken);

        if (existingSections.Count != sectionIds.Count)
            return RequestResult<List<UpdateSectionOrderOrActiveStatusResponse>>
                .Failure(ResultCode.SectionsNotFound); 
        
        var existingById = existingSections.ToDictionary(s => s.Id);
        try
        {
            foreach (var requestSection in request.Sections)
            {
                var section = new Section
                {
                    Id = requestSection.Id,
                    Index = requestSection.Index,
                    IsActive = requestSection.IsActive,
                };
                _repository.SaveInclude(section, nameof(Section.Index), nameof(Section.IsActive));
            }

            await _repository.SaveChangeAsync(cancellationToken);
        }
        catch
        {
            return RequestResult<List<UpdateSectionOrderOrActiveStatusResponse>>.Failure(ResultCode.CanNotUpdateSections);
        }

        var response = request.Sections
            .OrderBy(s => s.Index)
            .Select(requestSection =>
            {
                var existing = existingById[requestSection.Id];
                return new UpdateSectionOrderOrActiveStatusResponse(
                    existing.Id,
                    existing.Type.ToString(),
                    existing.Title,
                    requestSection.Index,
                    requestSection.IsActive,
                    existing.OccasionId,
                    existing.CategoryId);
            })
            .ToList();

        return RequestResult<List<UpdateSectionOrderOrActiveStatusResponse>>.succeeded(response,ResultCode.SectionsUpdatedSuccesfully);
    }
}
