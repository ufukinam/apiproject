using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyProject.Application.DTOs;
using MyProject.Core.Entities;

namespace MyProject.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Role, RoleDto>();
            // Add other mappings here
        }
    }
}