using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoList.DataAccess.DataContext;
using TodoList.DataAccess.Repositories;

namespace TodoList.DataAccess;

public static class DependencyInjection {
    public static IServiceCollection AddDataAccess(this IServiceCollection services) {
        // Add db context
        services.AddDbContext<AppDbContext>((sp, options) => {
            options.UseSqlServer(sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"));
        });

        // Add repositories
        services.AddScoped<ITodoItemRepository, TodoItemRepository>();

        return services;
    }
}
