using TodoApp.Models;

namespace TodoApp.Services;

public interface ITokensService
{
    string GenerateToken(User user);
    string CreateRefreshToken();
}