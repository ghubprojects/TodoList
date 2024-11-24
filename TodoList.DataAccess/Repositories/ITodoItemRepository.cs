using TodoList.Common.Models;

namespace TodoList.DataAccess.Repositories;

public interface ITodoItemRepository {
    Task<(List<TodoItem>, int)> GetAllAsync(string? searchText, bool? isCompleted, string? sortColumn, string? sortOrder, int page, int pageSize);
    Task<List<TodoItem>> GetAllAsync(List<int> ids);
    Task<TodoItem?> GetAsync(int id);
    Task AddAsync(TodoItem item);
    Task UpdateAsync(TodoItem item);
    Task UpdateMultipleAsync(List<TodoItem> items);
    Task DeleteAsync(TodoItem item);
    Task DeleteMultipleAsync(List<TodoItem> items);
}
