using TodoApp.Models.Shorts;

namespace TodoApp.Models
{
    public class PageNote : PageNoteShort
    {
        public List<ContentBase> Content { get; set; } = new List<ContentBase>();
        public string UserId { get; set; } = string.Empty;
    }
}
