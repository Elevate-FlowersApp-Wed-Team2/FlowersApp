using FlowersApp.Catalog.Domain.Enum;
using FloweryApp.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Catalog.Infrastructure.Persistence.Configurations;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasQueryFilter(s => !s.IsDeleted);
        builder.HasData(
  new Section
  {
      Id = Guid.NewGuid(), // Using Guid.NewGuid()
      Type = SectionType.ProductRail,
      Title = "Featured Products",
      Index = 1,
      IsActive = true,
      OccasionId = null,
      CategoryId = null,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow,
      CreatedBy = "System", // Provide a default value
      UpdatedBy = "System",
      IsDeleted = false
  },
                new Section
                {
                    Id = Guid.NewGuid(),
                    Type = SectionType.ProductRail,
                    Title = "New Arrivals",
                    Index = 2,
                    IsActive = true,
                    OccasionId = null,
                    CategoryId = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new Section
                {
                    Id = Guid.NewGuid(),
                    Type = SectionType.ProductRail,
                    Title = "Trending Now",
                    Index = 3,
                    IsActive = true,
                    OccasionId = null,
                    CategoryId = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },

                // Category Rail Sections
                new Section
                {
                    Id = Guid.NewGuid(),
                    Type = SectionType.CategoryRail,
                    Title = "Shop by Category",
                    Index = 4,
                    IsActive = true,
                    OccasionId = null,
                    CategoryId = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new Section
                {
                    Id = Guid.NewGuid(),
                    Type = SectionType.CategoryRail,
                    Title = "Popular Categories",
                    Index = 5,
                    IsActive = true,
                    OccasionId = null,
                    CategoryId = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },

                // Occasion Rail Sections
                new Section
                {
                    Id = Guid.NewGuid(),
                    Type = SectionType.OccasionRail,
                    Title = "Special Occasions",
                    Index = 6,
                    IsActive = true,
                    OccasionId = null,
                    CategoryId = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new Section
                {
                    Id = Guid.NewGuid(),
                    Type = SectionType.OccasionRail,
                    Title = "Holiday Collections",
                    Index = 7,
                    IsActive = true,
                    OccasionId = null,
                    CategoryId = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new Section
                {
                    Id = Guid.NewGuid(),
                    Type = SectionType.OccasionRail,
                    Title = "Birthday Specials",
                    Index = 8,
                    IsActive = true,
                    OccasionId = null,
                    CategoryId = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },

                // Banner Sections
                new Section
                {
                    Id = Guid.NewGuid(),
                    Type = SectionType.Banner,
                    Title = "Summer Sale",
                    Index = 9,
                    IsActive = true,
                    OccasionId = null,
                    CategoryId = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new Section
                {
                    Id = Guid.NewGuid(),
                    Type = SectionType.Banner,
                    Title = "Mother's Day Special",
                    Index = 10,
                    IsActive = true,
                    OccasionId = null,
                    CategoryId = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new Section
                {
                    Id = Guid.NewGuid(),
                    Type = SectionType.Banner,
                    Title = "Flash Sale",
                    Index = 11,
                    IsActive = true,
                    OccasionId = null,
                    CategoryId = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },

                // BestSeller Sections
                new Section
                {
                    Id = Guid.NewGuid(),
                    Type = SectionType.BestSeller,
                    Title = "Best Sellers",
                    Index = 12,
                    IsActive = true,
                    OccasionId = null,
                    CategoryId = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new Section
                {
                    Id = Guid.NewGuid(),
                    Type = SectionType.BestSeller,
                    Title = "Customer Favorites",
                    Index = 13,
                    IsActive = true,
                    OccasionId = null,
                    CategoryId = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new Section
                {
                    Id = Guid.NewGuid(),
                    Type = SectionType.BestSeller,
                    Title = "Top Rated",
                    Index = 14,
                    IsActive = true,
                    OccasionId = null,
                    CategoryId = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                }
            );
    }
}
