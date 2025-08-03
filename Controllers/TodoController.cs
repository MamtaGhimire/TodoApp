using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.DTOs;
using TodoApp.Services;

namespace TodoApp.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly TodoService _todoService;
    
    public TodoController(TodoService todoService)
    {
        _todoService = todoService;
    }

[HttpPost]
[Authorize]
public async Task<IActionResult> CreateTodos([FromBody] CreateTodoDto dto)
{
    var result = await _todoService.CreateTodoAsync(dto);
    return Ok(result);
}


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(string id, [FromBody] UpdateTodoDto dto)
    {
        var result = await _todoService.UpdateTodoAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(string id, [FromBody] UpdateTodoDto dto)
    {
        var result = await _todoService.DeleteTodoAsync(id);
        return Ok(result);

    }

    [HttpGet]
[Authorize]
public async Task<IActionResult> GetTodos()
{
    var result = await _todoService.GetTodosAsync();
    return Ok(result);
}

}