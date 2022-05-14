using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TodoApp.Models.Dto;

namespace TodoApp.Services
{
    public class MappingHelper : Profile
    {
        private readonly IMapper _mapper;

        public MappingHelper(IMapper mapper)
        {
            _mapper = mapper;

        }

        public UserDto getUser(IdentityUser user)
        {
            var t = _mapper.Map<UserDto>(user);
            return t;
        }
    }
}
