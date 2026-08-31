using FlowersApp.Catalog.Domain.Entities;
using FloweryApp.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FlowersApp.Catalog.Infrastructure.Persistence;

public class CatalogDbContext:DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        base.OnModelCreating(builder);
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Occasion> Occasions { get; set; }
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<CoverageArea> CoverageAreas => Set<CoverageArea>();
    public DbSet<CoverageCity> CoverageCities => Set<CoverageCity>();
    public DbSet<AddressStoreAssignment> AddressStoreAssignments => Set<AddressStoreAssignment>();
    public DbSet<Governorate> Governorates => Set<Governorate>();
    public DbSet<City> Cities => Set<City>();
}
