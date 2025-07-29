
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using TodoApp.DTOs;
using TodoApp.Helpers;
using TodoApp.Models;
using TodoApp.Services;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokensService _tokenservice;

    public UserService(IMongoDatabase database, IMapper mapper, IHttpContextAccessor httpContextAccessor, ITokensService tokensService)
    {
        _users = database.GetCollection<User>("Users");
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _tokenservice = tokensService;
    }

    public async Task<ServiceResponse<string>> RegisterAsync(UserRegisterDto registerDto)
    {
        var existingUser = await _users.Find(u => u.Email == registerDto.Email).FirstOrDefaultAsync();
        if (existingUser != null)
        {
            return new ServiceResponse<string>
            {
                IsSuccess = false,
                ErrorMessage = "Email already exists."
            };
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var newUser = new User
        {
            Name = registerDto.Name,
            Email = registerDto.Email,
            Username = registerDto.Username,
            PasswordHash = passwordHash
        };

        await _users.InsertOneAsync(newUser);

        return new ServiceResponse<string>
        {
            IsSuccess = true,
            Data = "User registered successfully"
        };
    }

    public async Task<ServiceResponse<AuthResponseDto>> LoginAsync(UserLoginDto loginDto)
    {
        var user = await _users.Find(u => u.Email == loginDto.Email).FirstOrDefaultAsync();

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return new ServiceResponse<AuthResponseDto>
            {
                IsSuccess = false,
                ErrorMessage = "Invalid email or password."
            };
        }

        string token = _tokenservice.GenerateToken(user);
        string refreshToken = _tokenservice.CreateRefreshToken();

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

    public async Task<ServiceResponse<UserResponseDto>> GetUserProfileAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return new ServiceResponse<UserResponseDto>
            {
                IsSuccess = false,
                ErrorMessage = "User ID not found in token."
            };
        }

        var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();

        if (user == null)
        {
            return new ServiceResponse<UserResponseDto>
            {
                IsSuccess = false,
                ErrorMessage = "User not found."
            };
        }

        return new ServiceResponse<UserResponseDto>
        {
            IsSuccess = true,
            Data = _mapper.Map<UserResponseDto>(user)
        };
    }

    public async Task<ServiceResponse<UserResponseDto>> UpdateUserProfileAsync(UpdateUserDto updateUserDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return new ServiceResponse<UserResponseDto>
            {
                IsSuccess = false,
                ErrorMessage = "User ID not found in token."
            };
        }

        var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();

        if (user == null)
        {
            return new ServiceResponse<UserResponseDto>
            {
                IsSuccess = false,
                ErrorMessage = "User not found."
            };
        }

        user.Name = updateUserDto.Name ?? user.Name;
        user.Email = updateUserDto.Email ?? user.Email;

        await _users.ReplaceOneAsync(u => u.Id == userId, user);

        return new ServiceResponse<UserResponseDto>
        {
            IsSuccess = true,
            Data = _mapper.Map<UserResponseDto>(user)
        };
    }
}

