using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Context;
using TodoApp.Models;
using TodoApp.Models.Dto.Requests;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "AppUser")]
    public class PageNoteController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public PageNoteController(ApiDbContext context)
        {
            _context = context;
        }
        
        // GET: api/<PageNoteController>
        [HttpGet]
        public async Task<ActionResult<List<PageNote>>> Get()
        {
            return Ok(await this._context.PageNotes.ToListAsync());
        }

        // GET api/<PageNoteController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PageNoteController>
        [HttpPost]
        public async Task<ActionResult<PageNote>> CreatePageNote([FromBody] PageNote pageNote)
        {
            if(ModelState.IsValid)
            {
                await _context.PageNotes.AddAsync(pageNote);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(CreatePageNote), pageNote);
            }
            return new JsonResult("Ok")
            {
                StatusCode = 500,
                ContentType = "TestType",
                SerializerSettings = { },
                Value = "NEW VALUE"
            };
        }

        // PUT api/<PageNoteController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PageNoteController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
