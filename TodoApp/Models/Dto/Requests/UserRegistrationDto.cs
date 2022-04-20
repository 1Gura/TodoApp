using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.Dto.Requests
{
    public class UserRegistrationDto
    {
        [Required]
        public string UserName { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "Длина пароля = 6 символов, пароль должен содержать символы алфавита и цифры")]
        public string Password { get; set; } = "";
        [Required]
        public string RepeatPassword { get; set; } = "";
    }
}
