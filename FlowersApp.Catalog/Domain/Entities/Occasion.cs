namespace FlowersApp.Catalog.Domain.Entities
{
    public class Occasion : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public List<Product> Products { get; set; } = new();
    }
}
