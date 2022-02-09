using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TodoApp.Context;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public TodoController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Todo>> GetTodosAsync()
        {
            _context.Todos.Add(new Todo() { Description = "awsdas", Title = "asdasd", Done = false });
            await _context.SaveChangesAsync();
            return Ok(await _context.Todos.ToListAsync());
        }
    }
}
