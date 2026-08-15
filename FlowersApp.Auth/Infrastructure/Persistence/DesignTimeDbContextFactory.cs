using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FlowersApp.Auth.Infrastructure.Persistence;

// Design-time factory used by EF tools to create AppDbContext without running the full application
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Try to read connection string from environment or appsettings
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables();

        var config = builder.Build();
        var conn = config.GetConnectionString("DefaultConnection")
            ?? config["DefaultConnection"]
            ?? "Server=(localdb)\\mssqllocaldb;Database=FlowersAppAuth_DesignTime;Trusted_Connection=True;TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(conn);
        return new AppDbContext(optionsBuilder.Options);
    }
}
