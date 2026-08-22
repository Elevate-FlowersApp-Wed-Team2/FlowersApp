using FlowersApp.Catalog.Domain.Enum;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using FloweryApp.Api.Domain.Entities;
using Mapster;

namespace FlowersApp.Catalog.Features.Home.CreateNewHomeSection;

public record CreateNewHomeSectionCommand
(string Type, string Title, int Index, bool IsActive
    , Guid? OccasionId, Guid? CategoryId): ICommand<CreateNewHomeSectionResponse>;

public record CreateNewHomeSectionResponse(Guid Id, string Type, string Title, int Index, bool IsActive
    , Guid? OccasionId, Guid? CategoryId);

public class CreateNewHomeSectionCommandHandler(Repository<Section> repository)
    : ICommandHandler<CreateNewHomeSectionCommand, CreateNewHomeSectionResponse>
{
    private readonly Repository<Section> _repository = repository;

    public async Task<RequestResult<CreateNewHomeSectionResponse>> Handle(CreateNewHomeSectionCommand request, CancellationToken cancellationToken)
    {
        Enum.TryParse<SectionType>(request.Type,true, out var sectionType);
        var section = new Section
        {
            Id = Guid.NewGuid(),
            Index = request.Index,
            IsActive = request.IsActive,
            OccasionId = request.OccasionId,
            CategoryId = request.CategoryId,
            Title = request.Title,
            Type = sectionType,
            IsDeleted = false,
        };
        _repository.Add(section);
       var affectedRows =  await _repository.SaveChangeAsync(cancellationToken);
        if (affectedRows != 1)
            return RequestResult<CreateNewHomeSectionResponse>.Failure(ResultCode.FailedToSaveSection);
        var result =section.Adapt<CreateNewHomeSectionResponse>();
        return RequestResult<CreateNewHomeSectionResponse>.succeeded(result, ResultCode.SectionSavedSuccesfully);
    }
}
