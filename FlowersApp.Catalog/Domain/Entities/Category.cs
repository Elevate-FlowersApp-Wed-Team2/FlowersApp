using FlowersApp.Catalog.Domain.Entities;

namespace FloweryApp.Api.Domain.Entities;

public sealed class Category : BaseEntity
{
    public string Name { get; set; }
    public  string IconUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public int? StoreId { get; set; }
}
