using TodoApp.Models;
using TodoApp.DTOs;

namespace TodoApp.Services
{
    public interface ITodoService
    {
        Task<List<Todo>> GetTodosAsync(string userId);
        Task<Todo> CreateTodoAsync(CreateTodoDto dto, string userId);
        Task<Todo> UpdateTodoAsync(string id, UpdateTodoDto dto, string userId);
        Task<bool> DeleteTodoAsync(string id, string userId);
    }
}