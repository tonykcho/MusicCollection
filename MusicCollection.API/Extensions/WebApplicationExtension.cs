using Microsoft.EntityFrameworkCore;

using MusicCollection.DataAccess.DbContexts;

namespace MusicCollection.API.Extensions;

public static class WebApplicationExtension
{
    public static async Task MigrateAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        await using (var scope = app.Services.CreateAsyncScope())
        {
            ILogger<WebApplication> logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();

            var context = scope.ServiceProvider.GetRequiredService<MusicCollectionDbContext>();

            try
            {
                logger.LogInformation("--> Start Migrating MusicCollectionDbContext");
                
                await context.Database.MigrateAsync(cancellationToken);
                
                logger.LogInformation("--> Migrate MusicCollectionDbContext Success");
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message, "--> An Error occured while migrating the database used on context");
                throw;
            }
        }
    }
}