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
    }
}

    
