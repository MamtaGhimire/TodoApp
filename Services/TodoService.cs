using System.Security.Claims;
using TodoApp.DTOs;
using TodoApp.Models;
using TodoApp.Repositories;
using TodoApp.Helpers;

using TodoApp.Services;
using AutoMapper;



namespace TodoApp.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TodoService> _logger;
        private readonly IMapper _mapper;

        public TodoService(ITodoRepository todoRepository, IHttpContextAccessor httpContextAccessor, ILogger<TodoService> logger, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _mapper = mapper;
        }

        private string? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User ID not found in token");
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

        public async Task<ServiceResponse<string>> AdminDeleteTodoAsync(string id)
        {
            var todo = await _todoRepository.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return new ServiceResponse<string>
                {
                    IsSuccess = false,
                    Message = "Todo not found"
                };
            }

            await _todoRepository.DeleteTodoByIdAsync(id);
            return new ServiceResponse<string>
            {
                Data = id,
                Message = "Todo deleted by admin"
            };
        }

        public async Task<ServiceResponse<List<TodoResponseDto>>> GetAllTodosAsync(int page, int pageSize, string? status, string? sortBy)
        {
            var allTodos = await _todoRepository.GetAllTodosAsync();

            // Filtering by status
            if (!string.IsNullOrEmpty(status))
            {
                allTodos = allTodos.Where(t => t.Status.ToLower() == status.ToLower()).ToList();
            }

            // Sorting by due date or title
            if (!string.IsNullOrEmpty(sortBy))
            {
                allTodos = sortBy.ToLower() switch
                {
                    "duedate" => allTodos.OrderBy(t => t.DueDate).ToList(),
                    "title" => allTodos.OrderBy(t => t.Title).ToList(),
                    _ => allTodos
                };
            }

            // Pagination
            var pagedTodos = allTodos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mapped = _mapper.Map<List<TodoResponseDto>>(pagedTodos);

            return new ServiceResponse<List<TodoResponseDto>>
            {
                Data = mapped,
                Message = $"Page {page} with {pageSize} items per page"
            };
        }

        public async Task<ServiceResponse<List<TodoResponseDto>>> GetMyTodosAsync(
    int page, int pageSize, string? status, string? sortBy)
        {
            string? userId = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User ID not found in context");
            }

            var allTodos = await _todoRepository.GetTodosByUserAsync(userId);

            // Filtering
            if (!string.IsNullOrEmpty(status))
                allTodos = allTodos.Where(t => t.Status == status).ToList();

            //  Sorting
            allTodos = sortBy switch
            {
                "title" => allTodos.OrderBy(t => t.Title).ToList(),
                "dueDate" => allTodos.OrderBy(t => t.DueDate).ToList(),
                _ => allTodos
            };

            // Pagination
            var paginated = allTodos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = _mapper.Map<List<TodoResponseDto>>(paginated);

            return new ServiceResponse<List<TodoResponseDto>>
            {
                Data = response,
                Message = "User's todos fetched successfully"
            };
        }
       
        public async Task<List<Todo>> GetTodosByUserAsync(string userId)
        {
            return await _todoRepository.GetTodosByUserAsync(userId);
        }


    }
}
