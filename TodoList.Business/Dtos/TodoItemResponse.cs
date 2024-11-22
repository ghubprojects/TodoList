namespace TodoList.Business.Dtos;

public sealed record TodoItemResponse(
    int Id,
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted);
