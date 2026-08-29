using FlowersApp.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Catalog.Infrastructure.Persistence.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.NameAr).HasMaxLength(100).IsRequired();
            builder.Property(c => c.NameEn).HasMaxLength(100).IsRequired();

            builder.HasOne(c => c.Governorate)
                .WithMany(g => g.Cities)
                .HasForeignKey(c => c.GovernorateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
