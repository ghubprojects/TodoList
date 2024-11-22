using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TodoList.Business.Services;

namespace TodoList.Business;

public static class DependencyInjection {
    public static IServiceCollection AddBusiness(this IServiceCollection services) {
        // Add auto mapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Add fluent validation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Add services
        services.AddScoped<ITodoItemService, TodoItemService>();

        return services;
    }
}
