using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RIIS.Academic.Infrastructure.Persistence.Seeders;

namespace RIIS.Academic.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeRiisAcademicDatabaseAsync(
        this IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<RiisAcademicDbContext>();

        await dbContext.Database.MigrateAsync(cancellationToken);

        await ParcoursAcademiqueSeeder.SeedAsync(
            dbContext,
            cancellationToken);
    }
}
