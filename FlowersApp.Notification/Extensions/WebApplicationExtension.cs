using FlowersApp.Notification.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Notification.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication ApplyDatabaseMigrations(this WebApplication app, ILogger logger)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
            dbContext.Database.Migrate();
            logger.LogInformation("Database migrations applied successfully for NotificationDb.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying database migrations in Notification Service: {Message}.", ex.Message);
        }

        return app;
    }
}
