using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Context
{
    public class ApiDbContext : DbContext
    {
        public virtual DbSet<Todo> Todos { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
            
        }

    }
}
