using FlowersApp.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Auth.Infrastructure.Persistence.Configurations;

public class VehicleConfigurations : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasOne(v => v.Driver)
               .WithOne(d => d.Vehicle)
               .HasForeignKey<Vehicle>(v => v.DriverId);
    }
}
