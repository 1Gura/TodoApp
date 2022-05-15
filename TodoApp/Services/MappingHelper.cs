using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TodoApp.Mapping;
using TodoApp.Models.Dto;

namespace TodoApp.Services
{
    public class MappingHelper : Profile
    {
        private readonly IMapper _mapper;

        public MappingHelper()
        {
            var mapperConfig = new MapperConfiguration(x =>
            {
                x.AddProfile<MappingUser>();
            });
            mapperConfig.AssertConfigurationIsValid();
            _mapper = mapperConfig.CreateMapper();
        }

        public UserDto getUser(IdentityUser user)
        {
            return _mapper.Map<UserDto>(user);
        }
    }
}
