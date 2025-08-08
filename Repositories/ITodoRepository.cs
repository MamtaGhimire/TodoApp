using TodoApp.Models;

namespace TodoApp.Repositories
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetUserTodosAsync(string userId);
        Task<Todo?> GetTodoByIdAsync(string id, string userId);
        Task AddTodoAsync(Todo todo);
        Task<bool> UpdateTodoAsync(Todo todo, string userId);
        Task<bool> DeleteTodoAsync(string id, string userId);
        Task<List<Todo>> GetAllTodosAsync();
        Task<List<Todo>> GetTodosByUserAsync(string userId);

        Task<Todo?> GetTodoByIdAsync(string id);

        Task DeleteTodoByIdAsync(string id);

    }
}
