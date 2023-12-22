using Dotnet.Homeworks.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> MigrateDBAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
        
        var notApplied = await dbContext!.Database.GetPendingMigrationsAsync();
        if (notApplied.Any())
            await dbContext.Database.MigrateAsync();
        return app;
    }
}
