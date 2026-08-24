using FlowersApp.Notification.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Notification.Infrastructure.Persistence
{
    public class NotificationDbContext:DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options): base(options)
        {

        }
        public DbSet<Domain.Entities.Notification> Notifications { get; set; }
        public DbSet<NotificationDelivery> NotificationDeliveries { get; set; }
        public DbSet<DeviceInstallation> DeviceInstallations { get; set; }
        public DbSet<NotificationTranslation> NotificationTranslations { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();

            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();

            return base.SaveChanges();
        }

        private void UpdateTimestamps()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }
        }
    }
}
