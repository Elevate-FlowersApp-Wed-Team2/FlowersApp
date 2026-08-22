using FlowersApp.Notification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Notification.Infrastructure.Persistence.Configurations
{
    public class NotificationDeliveryConfiguration : IEntityTypeConfiguration<NotificationDelivery>
    {
        public void Configure(EntityTypeBuilder<NotificationDelivery> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.AttemptCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.ProviderMessageId)
                .HasMaxLength(200);

            builder.Property(x => x.ErrorCode)
                .HasMaxLength(100);

            builder.Property(x => x.ErrorMessage)
                .HasMaxLength(1000);

            builder.HasIndex(x => new
            {
                x.NotificationId,
                x.DeviceInstallationId
            })
            .IsUnique();

            builder.HasIndex(x => new
            {
                x.Status,
                x.LastAttemptAt
            });

            builder.HasOne(x => x.Notification)
                .WithMany(x => x.Deliveries)
                .HasForeignKey(x => x.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.DeviceInstallation)
                .WithMany(x => x.NotificationDeliveries)
                .HasForeignKey(x => x.DeviceInstallationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
