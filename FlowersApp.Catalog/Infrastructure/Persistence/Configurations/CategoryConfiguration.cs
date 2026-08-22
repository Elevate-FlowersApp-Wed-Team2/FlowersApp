using FlowersApp.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Catalog.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.IconUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);

            builder.Property(c => c.SortOrder)
                .HasDefaultValue(0);

            // Same query pattern as Occasion — GetCategoriesHandler always
            // filters IsActive and orders by SortOrder together.
            builder.HasIndex(c => new { c.IsActive, c.SortOrder });

            // One-to-many with Product: a Category can be deleted only if no
            // Products reference it (or reassign them first) — Restrict prevents
            // an accidental cascade delete from wiping out products.
            builder.HasMany<Product>()
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
