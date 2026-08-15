using FlowersApp.Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication ApplyDatabaseMigrations(this WebApplication app ,ILogger logger)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying database migrations in Auth Service :{message}.",ex.Message);
        }

        return app;
    }
}
