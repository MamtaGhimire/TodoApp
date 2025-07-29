using TodoApp.DTOs;
using TodoApp.Helpers;

namespace TodoApp.Services
{
    public interface IUserService
    {
        public interface IUserService
        {
            Task<ServiceResponse<string>> RegisterAsync(UserRegisterDto registerDto);

            Task<ServiceResponse<AuthResponseDto>> LoginAsync(UserLoginDto dto);
            Task<ServiceResponse<UserResponseDto>> GetProfileAsync();
             Task<ServiceResponse<UserResponseDto>> UpdateUserProfileAsync(UpdateUserDto updateUserDto);

}

    }
}
