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
        public IActionResult GetTodosAsync()
        {

                _context.Todos.Add(new Todo { Id = 1, Title = "Заголовок", Description = "Описание", Done = false });
                _context.SaveChanges();
            
            return Ok(_context.Todos.ToList());
        }
    }
}
