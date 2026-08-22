using FlowersApp.Catalog.Domain.Entities;
using FloweryApp.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Catalog.Infrastructure.Persistence.Configurations
{
    public class OccasionConfiguration : IEntityTypeConfiguration<Occasion>
    {
        public void Configure(EntityTypeBuilder<Occasion> builder)
        {
            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(o => o.IsActive)
                .HasDefaultValue(true);

            builder.Property(o => o.SortOrder)
                .HasDefaultValue(0);

          
            builder.HasIndex(o => new { o.IsActive, o.SortOrder });
        }
    }
}
