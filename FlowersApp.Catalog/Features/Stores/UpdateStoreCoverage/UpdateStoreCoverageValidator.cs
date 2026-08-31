using FlowersApp.Catalog.Domain.Enum;
using FluentValidation;
using NetTopologySuite.Geometries;

namespace FlowersApp.Catalog.Features.Stores.UpdateStoreCoverage
{
    public class UpdateStoreCoverageValidator : AbstractValidator<UpdateStoreCoverageCommand>
    {
        public UpdateStoreCoverageValidator()
        {
            RuleFor(x => x.StoreId).NotEmpty();

            When(x => x.Type == CoverageType.Polygon, () =>
            {
                RuleFor(x => x.PolygonPoints)
                    .NotNull().WithMessage("Polygon points are required.")
                    .Must(p => p!.Count >= 3).WithMessage("A polygon needs at least 3 distinct points.");

                RuleFor(x => x).Must(HaveValidClosedPolygon)
                    .WithMessage("Polygon is not a valid closed shape (self-intersecting or degenerate).");
            });

            When(x => x.Type == CoverageType.Radius, () =>
            {
                RuleFor(x => x.CenterLatitude).NotNull().InclusiveBetween(-90, 90);
                RuleFor(x => x.CenterLongitude).NotNull().InclusiveBetween(-180, 180);
                RuleFor(x => x.RadiusMeters).NotNull().GreaterThan(0);
            });

            When(x => x.Type == CoverageType.CityList, () =>
            {
                RuleFor(x => x.Cities)
                    .NotNull().WithMessage("At least one city is required.")
                    .Must(c => c!.Count > 0);
            });
        }

        private bool HaveValidClosedPolygon(UpdateStoreCoverageCommand cmd)
        {
            if (cmd.PolygonPoints is null || cmd.PolygonPoints.Count < 3) return false;

            var coords = cmd.PolygonPoints
                .Select(p => new Coordinate(p.Longitude, p.Latitude))
                .ToList();

            // Auto-close the ring if the caller didn't repeat the first point.
            if (!coords.First().Equals2D(coords.Last()))
                coords.Add(coords.First());

            try
            {
                var polygon = new Polygon(new LinearRing(coords.ToArray()));
                return polygon.IsValid; // catches self-intersection, degenerate rings, etc.
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
