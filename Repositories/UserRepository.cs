using MongoDB.Driver;
using TodoApp.Models;

namespace TodoApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var result = await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
