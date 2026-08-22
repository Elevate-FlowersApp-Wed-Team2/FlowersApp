using FlowersApp.Cart.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Cart.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication ApplyDatabaseMigrations(this WebApplication app ,ILogger logger)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CartDbContext>();
            dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying database migrations in Auth Service :{message}.",ex.Message);
        }

        return app;
    }
}
