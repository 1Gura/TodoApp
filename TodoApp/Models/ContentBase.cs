namespace TodoApp.Models
{
    public class ContentBase
    {
        public int Id { get; set; } = 0;
        public string Content { get; set; } = string.Empty;
        public int? PageNoteId { get; set; }
    }
}
