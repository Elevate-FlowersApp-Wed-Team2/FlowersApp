using FloweryApp.Api.Common.Contracts;
using FloweryApp.Api.Domain.Interfaces;
using FloweryApp.Api.Features.Home.SectionBuilders;
using MediatR;

namespace FlowersApp.Catalog.Features.Home.GetHomeLayout;

public sealed class GetHomeLayoutHandler(
    IHomeSectionRepository homeSectionRepository,
    IEnumerable<IHomeSectionPayloadBuilder> payloadBuilders,
    ILogger<GetHomeLayoutHandler> logger)
    : IRequestHandler<GetHomeLayoutQuery, OperationResult<IReadOnlyList<HomeLayoutSectionDto>>>
{
    private readonly Dictionary<string, IHomeSectionPayloadBuilder> _buildersByType =
        payloadBuilders.ToDictionary(b => b.SectionType);

    public async Task<OperationResult<IReadOnlyList<HomeLayoutSectionDto>>> Handle(
        GetHomeLayoutQuery request,
        CancellationToken cancellationToken)
    {
        // Store-aware: only sections that are global or scoped to this store are even considered (AC8).
        var sections = await homeSectionRepository.GetEnabledForStoreAsync(request.StoreId, cancellationToken);

        var layout = new List<HomeLayoutSectionDto>(sections.Count);

        foreach (var section in sections)
        {
            if (!_buildersByType.TryGetValue(section.Type, out var builder))
            {
                // Backend-side mirror of AC3: an admin-entered type this build doesn't know how
                // to compose yet is skipped rather than sent half-formed. The client's own
                // "unknown type -> skip" handling is a second, independent safety net.
                logger.LogWarning("No section payload builder registered for type '{Type}' (section {Id})", section.Type, section.Id);
                continue;
            }

            var dto = await builder.BuildAsync(section, request.StoreId, cancellationToken);

            if (dto is not null)
            {
                layout.Add(dto);
            }
        }

        return OperationResult<IReadOnlyList<HomeLayoutSectionDto>>.Success(layout);
    }
}
