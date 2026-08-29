using FlowersApp.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Catalog.Infrastructure.Persistence.Configurations
{
    public class AddressStoreAssignmentConfiguration : IEntityTypeConfiguration<AddressStoreAssignment>
    {
        public void Configure(EntityTypeBuilder<AddressStoreAssignment> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasIndex(a => a.AddressId).IsUnique();
            builder.HasIndex(a => a.IsUnresolved);

            builder.HasOne(a => a.Store)
                .WithMany()
                .HasForeignKey(a => a.StoreId)
                .OnDelete(DeleteBehavior.SetNull); 
            // deactivating a store shouldn't cascade-delete history
        }
    }
}
