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
      ArabicTitle = "المنتجات المميزة",
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
                    ArabicTitle = "الوافدون الجدد",
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
                    ArabicTitle = "الأكثر رواجاً الآن",
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
                    ArabicTitle = "تسوق حسب الفئة",
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
                    ArabicTitle = "الفئات الشائعة",
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
                    ArabicTitle = "المناسبات الخاصة",
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
                    ArabicTitle = "مجموعات العطلات",
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
                    ArabicTitle = "عروض أعياد الميلاد",
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
                    ArabicTitle = "تخفيضات الصيف",
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
                    ArabicTitle = "عرض خاص بعيد الأم",
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
                    ArabicTitle = "تخفيضات سريعة",
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
                    ArabicTitle = "الأكثر مبيعاً",
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
                    ArabicTitle = "المفضلات لدى العملاء",
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
                    ArabicTitle = "الأعلى تقييماً",
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
