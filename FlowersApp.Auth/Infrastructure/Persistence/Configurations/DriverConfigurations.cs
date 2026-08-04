using FlowersApp.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Auth.Infrastructure.Persistence.Configurations;

public class DriverConfigurations : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("Drivers");
        builder.HasOne(d => d.Vehicle)
               .WithOne(v => v.Driver)
               .HasForeignKey<Driver>(d => d.VehicleId);

        builder.Property(c => c.PhoneNumber);
        builder.HasIndex(c => c.NormalizedEmail)
               .IsUnique()
               .HasFilter("[NormalizedEmail] IS NOT NULL");

        builder.HasIndex(c => c.NormalizedEmail)
            .HasDatabaseName("EmailIndex");
    }
}
