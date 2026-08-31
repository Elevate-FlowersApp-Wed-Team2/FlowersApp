using FlowersApp.Cart.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Cart.Infrastructure.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(a => a.IsDefault).HasDefaultValue(false);

            //builder.HasOne(a => a.City)
            //    .WithMany()
            //    .HasForeignKey(a => a.CityId)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => a.UserId)
                .IsUnique()
                .HasFilter("[IsDefault] = 1")
                .HasDatabaseName("UX_Addresses_UserId_IsDefault");
        }
    }
}
