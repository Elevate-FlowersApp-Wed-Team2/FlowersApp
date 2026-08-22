using FlowersApp.Notification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowersApp.Notification.Infrastructure.Persistence.Configurations
{
    public class NotificationTranslationConfiguration : IEntityTypeConfiguration<NotificationTranslation>
    {
        public void Configure(EntityTypeBuilder<NotificationTranslation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Language)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Body)
                .IsRequired()
                .HasMaxLength(2000);

            builder.HasIndex(x => new { x.NotificationId, x.Language })
                .IsUnique();

            builder.HasOne(x => x.Notification)
                .WithMany(x => x.Translations)
                .HasForeignKey(x => x.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
