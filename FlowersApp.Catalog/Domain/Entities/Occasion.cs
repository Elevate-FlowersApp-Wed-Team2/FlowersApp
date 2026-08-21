using FlowersApp.Catalog.Domain.Entities;

namespace FloweryApp.Api.Domain.Entities;

public sealed class Occasion :BaseEntity
{
    public  string Name { get; set; }
    public  string ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public class Occasion : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public List<Product> Products { get; set; } = new();
    }
}
