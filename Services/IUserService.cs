using TodoApp.DTOs;
using TodoApp.Helpers;
using System.Threading.Tasks;

namespace TodoApp.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<AuthResponseDto>> RegisterAsync(UserRegisterDto request);
        Task<ServiceResponse<AuthResponseDto>> LoginAsync(UserLoginDto request);
        Task<ServiceResponse<UserResponseDto>> GetUserProfileAsync(string userId);
        Task<ServiceResponse<UserResponseDto>> UpdateUserProfileAsync(string userId, UpdateUserDto request);
        Task<ServiceResponse<string>> SendOtpAsync(string email);
        Task<ServiceResponse<string>> VerifyOtpAsync(string email, string otpCode); 

        Task<ServiceResponse<string>> ForgotPasswordAsync(string email);
        Task<ServiceResponse<string>> ResetPasswordAsync(ResetPasswordDto request);
        
    }
}






    

