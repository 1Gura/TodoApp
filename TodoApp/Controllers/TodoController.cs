using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "AppUser")]
    public class TodoController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public TodoController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet("getTodos")]
        public async Task<ActionResult<Todo>> GetTodosAsync()
        {
            return Ok(await _context.Todos.ToListAsync());
        }

        [HttpGet("getTodoByTitle")]
        public async Task<ActionResult<List<Todo>>> GetTodosByTitle([FromQuery] string title)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    return BadRequest("Была передана нулевая строка");
                }
                var todos = await _context.Todos.Where(todo => todo.Title.Contains(title)).ToListAsync();
                if (!todos.Any())
                {
                    return BadRequest("Записей с таким заголовком нет");
                }
                return Ok(todos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("createTodo")]
        //[Authorize(Policy = "DepartmentPolicy")]
        public async Task<ActionResult<Todo>> CreateTodo([FromBody] Todo todo)
        {
            if (ModelState.IsValid)
            {
                await _context.Todos.AddAsync(todo);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateTodo), todo);

            }
            return new JsonResult("Ok")
            {
                StatusCode = 500,
                ContentType = "TestType",
                SerializerSettings = { },
                Value = "NEW VALUE"
            };
        }

        [HttpGet("getTodo/{id}")]
        public async Task<ActionResult<Todo>> GetItem(int id)
        {
            var todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (todo == null)
                return NotFound();
            return Ok(todo);
        }

        [HttpPatch("patchTodo/{id}")]
        public async Task<ActionResult<Todo>> UpdateTodo(int id, Todo todo)
        {
            if (id != todo.Id)
                return BadRequest("Запись которую вы пытаетесь изменить не существует");
            var existTodo = await _context.Todos.FirstOrDefaultAsync(y => y.Id == id);
            if (existTodo == null)
                return NotFound("Запись которую вы пытаетесь изменить не существует");
            existTodo.Title = todo.Title;
            existTodo.Description = todo.Description;
            existTodo.Done = todo.Done;
            await _context.SaveChangesAsync();
            return Ok(existTodo);
        }

        [HttpDelete("deleteTodo/{id}")]
        public async Task<ActionResult<Todo>> DeleteTodo(int id)
        {
            var existTodo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (existTodo == null)
                return NotFound("Запись которую вы пытаетесь удалить не существует");
            _context.Todos.Remove(existTodo);
            await _context.SaveChangesAsync();
            return Ok(existTodo);
        }
    }
}