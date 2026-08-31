using FlowersApp.Catalog.Domain.Enum;
using NetTopologySuite.Geometries;

namespace FlowersApp.Catalog.Domain.Entities
{
    public class CoverageArea : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public Store Store { get; set; } = default!;

        public CoverageType Type { get; set; }

        // Populated for BOTH Polygon and Radius (radius is converted to a circle polygon
        // on save) so spatial queries never need to branch on Type.
        public Polygon? Geometry { get; set; }

        // Kept only so Radius coverage can be re-edited without reverse-engineering the circle.
        public double? CenterLatitude { get; set; }
        public double? CenterLongitude { get; set; }
        public double? RadiusMeters { get; set; }

        public ICollection<CoverageCity> Cities { get; set; } = new List<CoverageCity>();

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }
}
