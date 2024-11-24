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
        var item = await FindExistingItemAsync(id);
        return _mapper.Map<TodoItemResponse>(item);
    }

    public async Task AddAsync(AddTodoItemCommand dto) {
        await new AddTodoItemCommandValidator().ValidateAndThrowAsync(dto);
        var item = _mapper.Map<TodoItem>(dto);
        await _todoItemRepository.AddAsync(item);
    }

    public async Task UpdateAsync(UpdateTodoItemCommand dto) {
        await new UpdateTodoItemCommandValidator().ValidateAndThrowAsync(dto);
        var existingItem = await FindExistingItemAsync(dto.Id);
        _mapper.Map(dto, existingItem);
        await _todoItemRepository.UpdateAsync(existingItem);
    }

    public async Task UpdateMultipleAsync(List<UpdateTodoItemCommand> dtos) {
        if (dtos.Count == 0) {
            throw new BadHttpRequestException("The list of dtos cannot be empty.");
        }

        var validator = new UpdateTodoItemCommandValidator();
        var validationTasks = dtos.Select(dto => validator.ValidateAndThrowAsync(dto));
        await Task.WhenAll(validationTasks);

        var ids = dtos.Select(dto => dto.Id).ToList();
        var existingItems = await FindExistingItemsAsync(ids);

        foreach (var dto in dtos) {
            var existingItem = existingItems.First(item => item.Id == dto.Id);
            _mapper.Map(dto, existingItem);
        }

        await _todoItemRepository.UpdateMultipleAsync(existingItems);
    }

    public async Task DeleteAsync(int id) {
        var item = await FindExistingItemAsync(id);
        await _todoItemRepository.DeleteAsync(item);
    }

    public async Task DeleteMultipleAsync(List<int> ids) {
        if (ids.Count == 0) {
            throw new BadHttpRequestException("The list of IDs cannot be empty.");
        }

        var existingItems = await FindExistingItemsAsync(ids);
        await _todoItemRepository.DeleteMultipleAsync(existingItems);
    }

    private async Task<TodoItem> FindExistingItemAsync(int id) {
        return await _todoItemRepository.GetAsync(id)
            ?? throw new BadHttpRequestException($"The Todo item with ID {id} does not exist.");
    }

    private async Task<List<TodoItem>> FindExistingItemsAsync(List<int> ids) {
        var existingItems = await _todoItemRepository.GetAllAsync(ids);
        var missingIds = ids.Except(existingItems.Select(item => item.Id)).ToList();

        if (missingIds.Count != 0)
            throw new BadHttpRequestException($"The following Todo items do not exist: {string.Join(", ", missingIds)}");

        return existingItems;
    }
}