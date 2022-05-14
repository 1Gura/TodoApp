using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TodoApp.Models.Dto;

namespace TodoApp.Mapping
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<IdentityUser, UserDto>();
                //.ForMember(x => x.Id, o=>o.Ignore());
        }
    }
}
