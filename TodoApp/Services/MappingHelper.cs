using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TodoApp.Mapping;
using TodoApp.Models;
using TodoApp.Models.Dto;
using TodoApp.Models.Shorts;

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
                x.AddProfile<MappingPageNote>();
            });
            mapperConfig.AssertConfigurationIsValid();
            _mapper = mapperConfig.CreateMapper();
        }

        public UserDto getUser(IdentityUser user)
        {
            return _mapper.Map<UserDto>(user);
        }

        public List<PageNoteShort> getPageNotesShort(List<PageNote> pageNotes)
        {
            List<PageNoteShort> listPageNote = new List<PageNoteShort>();
            foreach (PageNote pageNote in pageNotes)
            {
                listPageNote.Add(_mapper.Map<PageNoteShort>(pageNote));
            }
            return listPageNote;
        }
    }
}
