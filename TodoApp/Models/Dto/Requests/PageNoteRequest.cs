using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.Dto.Requests
{
    public class PageNoteRequest
    {
        [Required]
        public string Title { get; set; } = "";
        [Required]
        public string Content { get; set; } = "";
        [Required]
        public string UserId { get; set; } = "";
    }
}
