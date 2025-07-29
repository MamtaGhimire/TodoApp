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
    public async Task<IActionResult> CreateTodo([FromBody] CreateTodoDto dto)
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoById(string id)
    {
        var result = await _todoService.GetTodoByIdAsync(id);
        return Ok(result);
    }
}