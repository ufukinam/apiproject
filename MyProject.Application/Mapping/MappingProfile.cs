using AutoMapper;
using MyProject.Application.DTOs;
using MyProject.Core.Entities;

namespace MyProject.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Page, PageDto>().ReverseMap();
            // Add other mappings here
        }
    }
}