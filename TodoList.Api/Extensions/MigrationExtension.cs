using Microsoft.EntityFrameworkCore;
using TodoList.DataAccess.DataContext;

namespace TodoList.Api.Extensions;

public static class MigrationExtension {
    public static void ApplyMigrations(this WebApplication app) {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}
