using Microsoft.AspNetCore.Mvc;
using TodoList.Business.Dtos;
using TodoList.Business.Services;

namespace TodoList.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController(ITodoItemService todoItemService) : ControllerBase {

    private readonly ITodoItemService _todoItemService = todoItemService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllTodoItemQuery dto) {
        var result = await _todoItemService.GetAllAsync(dto);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id) {
        var result = await _todoItemService.GetAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddTodoItemCommand dto) {
        await _todoItemService.AddAsync(dto);
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTodoItemCommand dto) {
        if (id != dto.Id) {
            throw new BadHttpRequestException("The provided ID does not match the dto ID.");
        }

        await _todoItemService.UpdateAsync(dto);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMultiple([FromBody] List<UpdateTodoItemCommand> dtos) {
        await _todoItemService.UpdateMultipleAsync(dtos);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        await _todoItemService.DeleteAsync(id);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMultiple([FromBody] List<int> ids) {
        await _todoItemService.DeleteMultipleAsync(ids);
        return NoContent();
    }
}