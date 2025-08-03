using TodoApp.Models;

namespace TodoApp.Services;

public interface ITokensService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}