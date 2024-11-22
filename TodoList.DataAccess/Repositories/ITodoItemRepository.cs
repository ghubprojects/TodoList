using TodoList.Common.Models;

namespace TodoList.DataAccess.Repositories;

public interface ITodoItemRepository {
    Task<(List<TodoItem>, int)> GetAllAsync(string? searchText, bool? isCompleted, string? sortColumn, string? sortOrder, int page, int pageSize);
    Task<TodoItem?> GetAsync(int id);
    Task AddAsync(TodoItem todoItem);
    Task UpdateAsync(TodoItem todoItem);
    Task DeleteAsync(TodoItem todoItem);
}
