using AutoMapper;
using TodoApp.Models;
using TodoApp.Models.Shorts;

namespace TodoApp.Mapping
{
    public class MappingPageNote : Profile
    {
        public MappingPageNote()
        {
            CreateMap<PageNote, PageNoteShort>();
        }

    }
}
