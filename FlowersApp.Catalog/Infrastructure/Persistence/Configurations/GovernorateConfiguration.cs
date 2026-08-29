using FlowersApp.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Catalog.Infrastructure.Persistence.Configurations
{
    public class GovernorateConfiguration : IEntityTypeConfiguration<Governorate>
    {
        public void Configure(EntityTypeBuilder<Governorate> builder)
        {
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Id).ValueGeneratedNever(); // IDs come from seed data, not identity
            builder.Property(g => g.NameAr).HasMaxLength(100).IsRequired();
            builder.Property(g => g.NameEn).HasMaxLength(100).IsRequired();
        }
    }

   
}
