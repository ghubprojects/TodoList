using TodoList.Business.Dtos;
using TodoList.Common.Wrappers;

namespace TodoList.Business.Services;

public interface ITodoItemService {
    Task<PagedList<TodoItemResponse>> GetAllAsync(GetAllTodoItemQuery query);
    Task<TodoItemResponse> GetAsync(int id);
    Task AddAsync(AddTodoItemCommand command);
    Task UpdateAsync(UpdateTodoItemCommand command);
    Task UpdateMultipleAsync(List<UpdateTodoItemCommand> dtos);
    Task DeleteAsync(int id);
    Task DeleteMultipleAsync(List<int> ids);
}