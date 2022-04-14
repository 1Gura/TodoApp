using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models.Dto
{
    public class RefreshToken
    {
        public int Id { get; set; } = 0;
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string JwtId { get; set; } = string.Empty;
        public bool IsUsed { get; set; } = false;
        public bool IsRevorked { get; set; } = false;
        public DateTime AddedDate { get; set; } = new DateTime();
        public DateTime ExpiryDate { get; set; } = new DateTime();

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = new IdentityUser();

    }
}
