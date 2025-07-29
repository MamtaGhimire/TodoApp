using TodoApp.Models;
using TodoApp.DTOs;
using TodoApp.Validators;
using MongoDB.Driver;
using TodoApp.Helpers;

namespace TodoApp.Services;

public class TodoService
{
    private readonly IMongoCollection<Todo> _todos;

    public TodoService(IMongoDatabase database)
    {
        _todos = database.GetCollection<Todo>("Todos");
    }

    public async Task<Todo> CreateTodoAsync(CreateTodoDto dto)
    {
        var todo = new Todo
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            DueDate = dto.DueDate,
            CreatedAt = DateTime.UtcNow
        };

        await _todos.InsertOneAsync(todo);
        return todo;
    }

    public async Task<ServiceResponse<Todo>> UpdateTodoAsync(string id, UpdateTodoDto dto)
    {
        var response = new ServiceResponse<Todo>();

        var todo = await _todos.Find(t => t.Id == id).FirstOrDefaultAsync();
        if (todo == null)
        {
            response.IsSuccess = false;
            response.ErrorMessage = "Todo not found";
            return response;
        }

        todo.Title = dto.Title;
        todo.Description = dto.Description;
        todo.Status = dto.Status;
        todo.DueDate = dto.DueDate;

        await _todos.ReplaceOneAsync(t => t.Id == id, todo);

        response.Data = todo;
        response.Message = "Todo updated successfully";
        return response;
    }

    public async Task<bool> DeleteTodoAsync(string id)
    {
        var result = await _todos.DeleteOneAsync(t => t.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<List<Todo>> GetAllTodosAsync()
    {
        return await _todos.Find(_ => true).ToListAsync();
    }

    public async Task<Todo?> GetTodoByIdAsync(string id)
    {
        return await _todos.Find(t => t.Id == id).FirstOrDefaultAsync();
    }
}
