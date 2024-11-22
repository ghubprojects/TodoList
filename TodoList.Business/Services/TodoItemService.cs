using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using TodoList.Business.Dtos;
using TodoList.Common.Models;
using TodoList.Common.Wrappers;
using TodoList.DataAccess.Repositories;

namespace TodoList.Business.Services;

public class TodoItemService(ITodoItemRepository todoItemRepository, IMapper mapper) : ITodoItemService {
    private readonly ITodoItemRepository _todoItemRepository = todoItemRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedList<TodoItemResponse>> GetAllAsync(GetAllTodoItemQuery query) {
        await new GetAllTodoQueryValidator().ValidateAndThrowAsync(query);

        var (items, totalCount) = await _todoItemRepository.GetAllAsync(query.SearchText, query.IsCompleted, query.SortColumn, query.SortOrder, query.Page, query.PageSize);

        var itemResponse = _mapper.Map<List<TodoItemResponse>>(items);
        return new PagedList<TodoItemResponse>(itemResponse, query.Page, query.PageSize, totalCount);
    }

    public async Task<TodoItemResponse> GetAsync(int id) {
        var item = await _todoItemRepository.GetAsync(id)
            ?? throw new BadHttpRequestException("The Todo item does not exist.");
        return _mapper.Map<TodoItemResponse>(item);
    }

    public async Task AddAsync(AddTodoItemCommand command) {
        await new AddTodoItemCommandValidator().ValidateAndThrowAsync(command);
        var item = _mapper.Map<TodoItem>(command);
        await _todoItemRepository.AddAsync(item);
    }

    public async Task UpdateAsync(UpdateTodoItemCommand command) {
        await new UpdateTodoItemCommandValidator().ValidateAndThrowAsync(command);
        var todoItem = await _todoItemRepository.GetAsync(command.Id)
            ?? throw new BadHttpRequestException("The Todo item does not exist.");
        var item = _mapper.Map(command, todoItem);
        await _todoItemRepository.UpdateAsync(item);
    }

    public async Task DeleteAsync(int id) {
        var todoItem = await _todoItemRepository.GetAsync(id)
             ?? throw new BadHttpRequestException("The Todo item does not exist.");
        await _todoItemRepository.DeleteAsync(todoItem);
    }
}
