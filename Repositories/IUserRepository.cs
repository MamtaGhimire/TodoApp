using TodoApp.Models;
using System.Threading.Tasks;

namespace TodoApp.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(string id);
        Task CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
    }
}
