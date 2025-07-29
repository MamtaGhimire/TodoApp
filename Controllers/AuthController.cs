using Microsoft.AspNetCore.Mvc;
using TodoApp.DTOs;
using TodoApp.Services;
using TodoApp.Helpers;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            var result = await _userService.RegisterAsync(registerDto);

            if (result == null || !result.IsSuccess)
            {
                return BadRequest(new { message = result?.ErrorMessage ?? "Registration failed" });
            }

            return Ok(new { message = result.Data });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);

            if (result == null || !result.IsSuccess)
            {
                return Unauthorized(new { message = result?.ErrorMessage ?? "Login failed" });
            }

            if (result.Data == null)
            {
                return BadRequest(new { message = "Login succeeded but no token data returned." });
            }

            return Ok(new
            {
                token = result.Data.Token,
                refreshToken = result.Data.RefreshToken
            });
        }

        [HttpGet("profile")]
        public async Task<ActionResult<ServiceResponse<UserResponseDto>>> GetProfile()
        {
            var result = await _userService.GetUserProfileAsync();
            return Ok(result);
        }

        [HttpPut("profile")]
        public async Task<ActionResult<ServiceResponse<UserResponseDto>>> UpdateProfileAsync(UpdateUserDto updateUserDto)
        {
            var result = await _userService.UpdateUserProfileAsync(updateUserDto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result);
        }
    }
}
