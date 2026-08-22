using FlowersApp.Notification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Notification.Infrastructure.Persistence.Configurations
{
    public class DeviceInstallationConfiguration : IEntityTypeConfiguration<DeviceInstallation>
    {
        public void Configure(EntityTypeBuilder<DeviceInstallation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DeviceId)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.FcmToken)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Platform)
                .IsRequired();

            builder.Property(x => x.Language)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.AppVersion)
                .HasMaxLength(50);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(x => new { x.UserId, x.DeviceId })
                .IsUnique();

            builder.HasIndex(x => new { x.UserId, x.IsActive });

            builder.HasIndex(x => x.FcmToken);

            builder.HasMany(x => x.NotificationDeliveries)
                .WithOne(x => x.DeviceInstallation)
                .HasForeignKey(x => x.DeviceInstallationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
