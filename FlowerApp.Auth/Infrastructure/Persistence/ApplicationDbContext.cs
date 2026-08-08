using FlowerApp.Auth.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FlowerApp.Auth.Infrastructure.Persistence
{
    public class ApplicationDbContext
     : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GuestSession> GuestSessions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<PasswordResetOtp> PasswordResetOtps { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RefreshToken>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RefreshToken>()
                .HasOne(x => x.GuestSession)
                .WithMany()
                .HasForeignKey(x => x.GuestSessionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
