using FlowersApp.Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Infrastructure.Persistence;

public class AppDbContext:DbContext
{
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<DriverApplication> Applications { get; set; }
    public DbSet<DriverDocument> Documents { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
