
using MongoDB.Driver;
using TodoApp.Models;


namespace TodoApp.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IMongoCollection<Todo> _todos;
        private readonly IMongoCollection<Todo> _todoCollection;


        public TodoRepository(IMongoDatabase database)
        {
            _todos = database.GetCollection<Todo>("Todos");
            _todoCollection = database.GetCollection<Todo>("Todos");
        }

        public async Task<List<Todo>> GetUserTodosAsync(string userId)
        {
            return await _todos.Find(t => t.UserId == userId).ToListAsync();
        }

        public async Task<Todo?> GetTodoByIdAsync(string id, string userId)
        {
            return await _todos.Find(t => t.Id == id && t.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task AddTodoAsync(Todo todo)
        {
            await _todos.InsertOneAsync(todo);
        }

        public async Task<bool> UpdateTodoAsync(Todo todo, string userId)
        {
            var result = await _todos.ReplaceOneAsync(
                t => t.Id == todo.Id && t.UserId == userId,
                todo
            );

            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteTodoAsync(string id, string userId)
        {
            var result = await _todos.DeleteOneAsync(t => t.Id == id && t.UserId == userId);
            return result.DeletedCount > 0;
        }
        public async Task<List<Todo>> GetAllTodosAsync()

        {
            return await _todoCollection.Find(_ => true).ToListAsync();

        }


        public async Task<Todo?> GetTodoByIdAsync(string id)
        {
            return await _todoCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        }


        public async Task DeleteTodoByIdAsync(string id)
        {
            await _todoCollection.DeleteOneAsync(t => t.Id == id);
        }


        public async Task<List<Todo>> GetTodosByUserAsync(string userId)
        {
            return await _todoCollection.Find(t => t.UserId == userId).ToListAsync();
        }

    }
}
