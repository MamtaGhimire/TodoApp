using System.Security.Claims;
using TodoApp.DTOs;
using TodoApp.Models;
using TodoApp.Repositories;
using TodoApp.Helpers;
using TodoApp.Services;



namespace TodoApp.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TodoService> _logger;

        public TodoService(ITodoRepository todoRepository, IHttpContextAccessor httpContextAccessor, ILogger<TodoService> logger)
        {
            _todoRepository = todoRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        private string? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<ServiceResponse<Todo>> CreateTodoAsync(CreateTodoDto dto)
        {
            var response = new ServiceResponse<Todo>();
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Unauthorized";
                return response;
            }

            var todo = new Todo
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                DueDate = dto.DueDate,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            await _todoRepository.AddTodoAsync(todo);
            response.Data = todo;
            response.Message = "Todo created successfully";
            return response;
        }

        public async Task<ServiceResponse<List<Todo>>> GetTodosAsync()
        {
            var response = new ServiceResponse<List<Todo>>();
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                response.IsSuccess = false;
                response.ErrorMessage = "User not authenticated.";
                return response;
            }

            var todos = await _todoRepository.GetUserTodosAsync(userId);
            response.Data = todos;
            return response;
        }

        public async Task<ServiceResponse<Todo>> UpdateTodoAsync(string id, UpdateTodoDto dto)
        {
            var response = new ServiceResponse<Todo>();
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Unauthorized";
                return response;
            }

            var todo = await _todoRepository.GetTodoByIdAsync(id, userId);
            if (todo == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Todo not found or access denied";
                return response;
            }

            todo.Title = dto.Title;
            todo.Description = dto.Description;
            todo.Status = dto.Status;
            todo.DueDate = dto.DueDate;

            var updated = await _todoRepository.UpdateTodoAsync(todo, userId);
            if (!updated)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Failed to update todo";
                return response;
            }

            response.Data = todo;
            response.Message = "Todo updated successfully";
            return response;
        }

        public async Task<ServiceResponse<string>> DeleteTodoAsync(string id)
        {
            var response = new ServiceResponse<string>();
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Unauthorized";
                return response;
            }

            var deleted = await _todoRepository.DeleteTodoAsync(id, userId);
            if (!deleted)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Todo not found or access denied";
                return response;
            }

            response.Data = id;
            response.Message = "Todo deleted successfully";
            return response;
        }
    }
}
