namespace TodoApp.Models
{
    public class PageNote
    {
        public int Id { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public List<ContentBase> Content { get; set; } = new List<ContentBase>();
        public string UserId { get; set; } = string.Empty;
    }
}
