using Microsoft.AspNetCore.Mvc;
using TodoList.Business.Dtos;
using TodoList.Business.Services;

namespace TodoList.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController(ITodoItemService todoItemService) : ControllerBase {
    private readonly ITodoItemService _todoItemService = todoItemService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllTodoItemQuery query) {
        var result = await _todoItemService.GetAllAsync(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoItem([FromRoute] int id) {
        var result = await _todoItemService.GetAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddTodoItemCommand command) {
        await _todoItemService.AddAsync(command);
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateToDoItem([FromRoute] int id, [FromBody] UpdateTodoItemCommand command) {
        if (id != command.Id)
            return BadRequest("Id mismatch");

        await _todoItemService.UpdateAsync(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteToDoItem([FromRoute] int id) {
        await _todoItemService.DeleteAsync(id);
        return NoContent();
    }
}