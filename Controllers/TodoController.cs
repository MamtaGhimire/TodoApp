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

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminEndpoint()
    {
        return Ok("Hello Admin!");
    }

    [Authorize(Roles = "User")]
    [HttpGet("user-only")]
    public IActionResult UserEndpoint()
    {
        return Ok("Hello User!");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("admin-delete/{id}")]
    public async Task<IActionResult> AdminDeleteTodo(string id)
    {
        var response = await _todoService.AdminDeleteTodoAsync(id);
        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllTodos(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? status = null,
    [FromQuery] string? sortBy = null)
    {
        var response = await _todoService.GetAllTodosAsync(page, pageSize, status, sortBy);
        return Ok(response);
    }


    [HttpGet("mytodos")]

    [Authorize]

    public async Task<IActionResult> GetMyTodos(
    int page = 1,
    int pageSize = 10,
    string? status = null,
    string? sortBy = null)
    {
        var response = await _todoService.GetMyTodosAsync(page, pageSize, status, sortBy);
        return Ok(response);
    }



}