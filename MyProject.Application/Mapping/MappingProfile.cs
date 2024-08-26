using AutoMapper;
using MyProject.Application.DTOs;
using MyProject.Core.Entities;
using MyProject.Core.Models;

namespace MyProject.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ForMember(dest => dest.Roles,
                m => m.MapFrom(src =>  string.Join(',', src.UserRoles)));
            CreateMap<UserDto, User>().ForMember(dest => dest.UserRoles,
                m => m.MapFrom(src => src.Roles.Split(',', System.StringSplitOptions.RemoveEmptyEntries).ToList()));
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Page, PageDto>().ReverseMap();
            CreateMap<UserRole, UserRoleDto>().ReverseMap();
            CreateMap<PaginatedResult<User>, PaginatedResult<UserDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)).ReverseMap();
        }
    }
}