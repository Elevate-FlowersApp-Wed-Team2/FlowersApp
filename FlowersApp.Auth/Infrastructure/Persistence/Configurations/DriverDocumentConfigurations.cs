using FlowersApp.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Auth.Infrastructure.Persistence.Configurations;

public class DriverDocumentConfigurations : IEntityTypeConfiguration<DriverDocument>
{
    public void Configure(EntityTypeBuilder<DriverDocument> builder)
    {
        builder.HasOne(d => d.DriverApplication)
            .WithMany(d => d.Documents)
            .HasForeignKey(d => d.ApplicationId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
