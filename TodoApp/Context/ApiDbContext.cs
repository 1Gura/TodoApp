using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Models.Dto;

namespace TodoApp.Context
{
    public class ApiDbContext : IdentityDbContext
    {
        public virtual DbSet<Todo> Todos { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<PageNote> PageNotes { get; set; }
        public virtual DbSet<ContentBase> Contents { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
