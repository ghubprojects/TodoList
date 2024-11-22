using AutoMapper;
using TodoList.Business.Dtos;
using TodoList.Common.Models;

namespace TodoList.Business.Profiles;

public class MappingProfile : Profile {
    public MappingProfile() {
        CreateMap<TodoItem, TodoItemResponse>();
        CreateMap<AddTodoItemCommand, TodoItem>();
        CreateMap<UpdateTodoItemCommand, TodoItem>();
    }
}
