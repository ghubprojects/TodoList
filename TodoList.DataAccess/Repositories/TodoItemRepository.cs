using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoList.Common.Models;
using TodoList.DataAccess.DataContext;

namespace TodoList.DataAccess.Repositories;

public class TodoItemRepository(AppDbContext context) : ITodoItemRepository {

    private readonly AppDbContext _context = context;

    public async Task<(List<TodoItem>, int)> GetAllAsync(string? searchText, bool? isCompleted, string? sortColumn, string? sortOrder, int page, int pageSize) {
        var query = _context.TodoItems.AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchText)) {
            query = query.Where(x => x.Title.Contains(searchText) || x.Description.Contains(searchText));
        }
        if (isCompleted.HasValue) {
            query = query.Where(item => item.IsCompleted == isCompleted.Value);
        }

        // Sorting
        Expression<Func<TodoItem, object>> sortProperty = sortColumn?.ToLower() switch {
            "title" => x => x.Title,
            "duedate" => x => x.DueDate,
            _ => x => x.Id
        };
        query = sortOrder?.ToLower() == "desc"
            ? query = query.OrderByDescending(sortProperty)
            : query = query.OrderBy(sortProperty);

        // Paging
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<TodoItem>> GetAllAsync(List<int> ids) {
        return await _context.TodoItems
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }

    public async Task<TodoItem?> GetAsync(int id) {
        return await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(TodoItem item) {
        item.CreatedAt = DateTime.Now;
        item.UpdatedAt = DateTime.Now;
        await _context.TodoItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TodoItem item) {
        item.UpdatedAt = DateTime.Now;
        _context.TodoItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMultipleAsync(List<TodoItem> items) {
        items.ForEach(x => x.UpdatedAt = DateTime.Now);
        _context.TodoItems.UpdateRange(items);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TodoItem item) {
        item.IsDeleted = true;
        _context.TodoItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMultipleAsync(List<TodoItem> items) {
        items.ForEach(x => x.IsDeleted = true);
        _context.TodoItems.UpdateRange(items);
        await _context.SaveChangesAsync();
    }
}
