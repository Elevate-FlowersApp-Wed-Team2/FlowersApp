using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Notification.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Domain.Entities.Notification>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Notification> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .IsRequired();

            builder.Property(x => x.Source)
                .IsRequired();

            builder.Property(x => x.Payload)
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.Status)
                .IsRequired();

            builder.HasMany(x => x.Translations)
                .WithOne(x => x.Notification)
                .HasForeignKey(x => x.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Deliveries)
                .WithOne(x => x.Notification)
                .HasForeignKey(x => x.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
