namespace FlowersApp.Catalog.Domain.Entities
{
    public class CoverageCity : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CoverageAreaId { get; set; }
        public CoverageArea CoverageArea { get; set; } = default!;
        public string CityName { get; set; } = default!;
        public string? Region { get; set; }
    }
}
