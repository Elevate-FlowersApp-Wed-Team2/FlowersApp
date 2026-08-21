using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Domain.Enum;

namespace FloweryApp.Api.Domain.Entities;

public sealed class Section : BaseEntity
{
    public SectionType Type { get; set; }
    public string Title { get; set; }
    public string ArabicTitle { get; set; }
    public int Index { get; set; }
    public bool IsActive { get; set; }
    public Guid? OccasionId { get; set; }
    public Guid? CategoryId { get; set; }

    public Occasion? Occasion { get; set; }
    public Category? Category { get; set; }


}
