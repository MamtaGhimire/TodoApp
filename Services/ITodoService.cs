using TodoApp.Models;
using TodoApp.DTOs;
using TodoApp.Helpers;

namespace TodoApp.Services
{
     public interface ITodoService
    {
        Task<ServiceResponse<Todo>> CreateTodoAsync(CreateTodoDto dto);
        Task<ServiceResponse<List<Todo>>> GetTodosAsync();
        Task<ServiceResponse<Todo>> UpdateTodoAsync(string id, UpdateTodoDto dto);
        Task<ServiceResponse<string>> DeleteTodoAsync(string id);

        // Admin-specific
        Task<ServiceResponse<List<TodoResponseDto>>> GetAllTodosAsync(int page, int pageSize, string? status, string? sortBy);
        Task<ServiceResponse<List<TodoResponseDto>>> GetMyTodosAsync(
    int page, int pageSize, string? status, string? sortBy);

        Task<List<Todo>> GetTodosByUserAsync(string userId);

        Task<ServiceResponse<string>> AdminDeleteTodoAsync(string id);
    }
}

    
