using FlowersApp.Catalog.Domain.Enum;

namespace FlowersApp.Catalog.Domain.Entities
{
    public class Store : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public StoreStatus Status { get; set; } = StoreStatus.Active;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<CoverageArea> CoverageAreas { get; set; } = new List<CoverageArea>();
    }
}
