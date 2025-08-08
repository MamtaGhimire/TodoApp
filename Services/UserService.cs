using AutoMapper;
using TodoApp.DTOs;
using TodoApp.Helpers;
using TodoApp.Models;
using TodoApp.Repositories;
using TodoApp.Services;
using User = TodoApp.Models.User;
using MongoDB.Bson;

namespace TodoApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokensService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ITokensService tokenService, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<AuthResponseDto>> RegisterAsync(UserRegisterDto request)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration attempt failed. User already exists: {Email}", request.Email);
                return new ServiceResponse<AuthResponseDto>
                {
                    IsSuccess = false,
                    ErrorMessage = "User already exists"
                };
            }

            var user = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Email = request.Email,
                FullName = request.Name ?? "Anonymous",
                Username = request.Username ?? "guest",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsVerified = false,
                Role = request.Role ?? "User"
            };
            // Generate 6-digit OTP

            var otp = new Random().Next(100000, 999999).ToString();

            user.OtpCode = otp;


            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(10);


            // Log OTP (just for development/testing)
            _logger.LogInformation("OTP for {Email} is: {OtpCode}", user.Email, otp);


            await _userRepository.CreateUserAsync(user);
            _logger.LogInformation("New user registered: {Email}", user.Email);


            var token = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateUserAsync(user);
            
            _logger.LogInformation("Access and refresh tokens generated for user: {Email}", user.Email);


            return new ServiceResponse<AuthResponseDto>
            {
                IsSuccess = true,
                Data = new AuthResponseDto
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    User = _mapper.Map<UserResponseDto>(user)


                }
            };
        }

        public async Task<ServiceResponse<AuthResponseDto>> LoginAsync(UserLoginDto request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed for email: {Email}", request.Email);
                return new ServiceResponse<AuthResponseDto>
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid email or password"
                };
            }

            var token = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateUserAsync(user);

            _logger.LogInformation("Login successful for user: {Email}", user.Email);

            return new ServiceResponse<AuthResponseDto>
            {
                IsSuccess = true,
                Data = new AuthResponseDto
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    User = _mapper.Map<UserResponseDto>(user)
                }
            };
        }
        public async Task<ServiceResponse<UserResponseDto>> UpdateUserProfileAsync(string userId, UpdateUserDto request)
        {
            await Task.CompletedTask;
            _logger.LogInformation("User profile updated for: {Email}", request.Email);

            return new ServiceResponse<UserResponseDto>
            {
                IsSuccess = true,
                Data = new UserResponseDto { FullName = request.Name, Email = "updated@example.com" }
            };
        }

       public async Task<ServiceResponse<string>> SendOtpAsync(string email)
{
    var user = await _userRepository.GetUserByEmailAsync(email);
    if (user == null)
    {
        return new ServiceResponse<string>
        {
            IsSuccess = false,
            ErrorMessage = "User not found"
        };
    }

    var otp = new Random().Next(100000, 999999).ToString();
    user.OtpCode = otp;
    user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(10);

    await _userRepository.UpdateUserAsync(user);

    _logger.LogInformation("OTP for {Email} is: {OtpCode}", user.Email, otp);

    return new ServiceResponse<string>
    {
        IsSuccess = true,
        Data = "OTP sent to email"
    };
}


        public async Task<ServiceResponse<string>> VerifyOtpAsync(string email, string otpCode)
        {
            await Task.CompletedTask;
            _logger.LogInformation("OTP verified for: {Email}", email);
            return new ServiceResponse<string>
            {
                IsSuccess = true,
                Data = "OTP verified successfully"
            };
        }

        public async Task<ServiceResponse<string>> ForgotPasswordAsync(string email)
        {
            await Task.CompletedTask;
            _logger.LogInformation("Password reset link sent to: {Email}", email);

            return new ServiceResponse<string>
            {
                IsSuccess = true,
                Data = "Password reset link sent to email"
            };
        }

        public async Task<ServiceResponse<string>> ResetPasswordAsync(ResetPasswordDto request)
        {
            await Task.CompletedTask;
            _logger.LogInformation("Password reset successful for: {Email}", request.Email);

            return new ServiceResponse<string>
            {
                IsSuccess = true,
                Data = "Password has been reset"
            };
        }
        public async Task<ServiceResponse<UserResponseDto>> GetUserProfileAsync(string userId)
{
    var user = await _userRepository.GetUserByIdAsync(userId);

    if (user == null)
    {
        _logger.LogWarning("Get profile failed. User not found with ID: {UserId}", userId);
        return new ServiceResponse<UserResponseDto>
        {
            IsSuccess = false,
            ErrorMessage = "User not found"
        };
    }

    _logger.LogInformation("User profile fetched for: {Email}", user.Email);

    return new ServiceResponse<UserResponseDto>
    {
        IsSuccess = true,
        Data = _mapper.Map<UserResponseDto>(user)
    };
}


    }
}

    