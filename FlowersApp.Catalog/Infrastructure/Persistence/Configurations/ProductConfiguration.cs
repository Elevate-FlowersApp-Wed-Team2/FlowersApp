using FlowersApp.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Catalog.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.DiscountPercentage)
                .HasColumnType("decimal(5,2)");
            builder.HasMany(p => p.Occasions)
                  .WithMany(o => o.Products)
                  .UsingEntity(j => j.ToTable("ProductOccasions"));
            builder.Property(p => p.ImageUrls)
                   .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList())
                    .HasMaxLength(2000);

            builder.Property(p => p.Includes)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList())
                .HasMaxLength(1000);

            builder.Property(p => p.Description)
                .HasMaxLength(2000);

        }
    }
}
