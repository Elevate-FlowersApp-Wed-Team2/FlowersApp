namespace FlowersApp.Catalog.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
    }
}
