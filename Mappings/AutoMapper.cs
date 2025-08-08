using AutoMapper;
using TodoApp.Models;
using TodoApp.DTOs;

namespace TodoApp.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap < Todo, TodoResponseDto>();
        }
    }
}