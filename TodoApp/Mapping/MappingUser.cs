using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TodoApp.Models.Dto;

namespace TodoApp.Mapping
{
    public class MappingUser : Profile
    {
        public MappingUser()
        {
            CreateMap<IdentityUser, UserDto>();
                //.ForMember(x => x.Id, o=>o.Ignore());
        }
    }
}
