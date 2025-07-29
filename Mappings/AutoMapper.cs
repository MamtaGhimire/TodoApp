using AutoMapper;
using TodoApp.Models;
using TodoApp.DTOs;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserResponseDto>();
    }
}