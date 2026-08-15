using FlowersApp.Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Infrastructure.Persistence;

public class AppDbContext:IdentityDbContext<AppUser, Role, Guid>
{
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<DriverApplication> Applications { get; set; }
    public DbSet<DriverDocument> Documents { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        // Enforce unique, case-insensitive email using NormalizedEmail (Identity uses normalization)
        builder.Entity<AppUser>(b =>
        {
            b.HasIndex(u => u.NormalizedEmail)
                .IsUnique()
                .HasDatabaseName("EmailIndex")
                .HasFilter("[NormalizedEmail] IS NOT NULL");

            b.HasIndex(u => u.PhoneNumber)
                .IsUnique()
                .HasDatabaseName("PhoneIndex")
                .HasFilter("[PhoneNumber] IS NOT NULL");
        });
    }
}
