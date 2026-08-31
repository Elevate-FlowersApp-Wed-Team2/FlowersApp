using FlowersApp.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Catalog.Infrastructure.Persistence.Configurations
{
    public class CoverageAreaConfiguration : IEntityTypeConfiguration<CoverageArea>
    {
        public void Configure(EntityTypeBuilder<CoverageArea> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Geometry)
                .HasColumnType("geography"); // SRID 4326 by default with UseNetTopologySuite()

            builder.HasMany(c => c.Cities)
                .WithOne(city => city.CoverageArea)
                .HasForeignKey(city => city.CoverageAreaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => c.IsActive);
        }
    }
}
