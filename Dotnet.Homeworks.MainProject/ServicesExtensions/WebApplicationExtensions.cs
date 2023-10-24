using Dotnet.Homeworks.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> MigrateDBAsync(this WebApplication app)
    {
        await using var dbContext = app.Services.GetService<AppDbContext>();

        var notApplied = await dbContext!.Database.GetPendingMigrationsAsync();
        if (notApplied.Any())
            await dbContext.Database.MigrateAsync();
        return app;
    }
}
