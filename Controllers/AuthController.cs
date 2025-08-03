using Microsoft.AspNetCore.Mvc;
using TodoApp.DTOs;
using TodoApp.Services;
using TodoApp.Helpers;
using TodoApp.Repositories;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
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
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in claims." });
            }
            var result = await _userService.GetUserProfileAsync(userId);
            return Ok(result);
        }

        [HttpPut("profile")]
        public async Task<ActionResult<ServiceResponse<UserResponseDto>>> UpdateProfileAsync(UpdateUserDto updateUserDto)
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in claims." });
            }

            var result = await _userService.UpdateUserProfileAsync(userId, updateUserDto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result);
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] string email)
        {
            var response = await _userService.SendOtpAsync(email);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto request)
        {
            var response = await _userService.VerifyOtpAsync(request.Email, request.OtpCode);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
           var response = await _userService.ForgotPasswordAsync(request.Email);
           if (!response.IsSuccess)
            return BadRequest(response);

            return Ok(response);
         }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
           var response = await _userService.ResetPasswordAsync(request);
           if (!response.IsSuccess)
            return BadRequest(response);

            return Ok(response);
        }


    };
}

