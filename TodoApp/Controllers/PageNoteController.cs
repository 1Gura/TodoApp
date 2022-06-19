using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Context;
using TodoApp.Mapping;
using TodoApp.Models;
using TodoApp.Models.Dto.Requests;
using TodoApp.Models.Shorts;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "AppUser")]
    public class PageNoteController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private MappingHelper mappingHelper { get; set; } = null;
        public PageNoteController(ApiDbContext context, MappingHelper mappingHelper)
        {
            _context = context;
            var mapperConfig = new MapperConfiguration(x =>
            {
                x.AddProfile<MappingPageNote>();
            });
            mapperConfig.AssertConfigurationIsValid();
            this.mappingHelper = mappingHelper;
        }

        // GET: api/<PageNoteController>
        [HttpGet]
        public async Task<ActionResult<List<PageNoteShort>>> Get()
        {
            var pageNotes = await this._context.PageNotes.ToListAsync();
            List<PageNoteShort> shortPageNotes = this.mappingHelper.getPageNotesShort(pageNotes);
            return Ok(shortPageNotes);
        }

        // GET api/<PageNoteController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PageNote>> Get(int id)
        {
            var pageNote = await this._context.PageNotes.FirstOrDefaultAsync(x => x.Id == id);
            if (pageNote == null)
            {
                return new JsonResult("Ok")
                {
                    StatusCode = 500,
                    ContentType = "TestType",
                    SerializerSettings = { },
                    Value = "NEW VALUE"
                };
            }

            var content = this._context.Contents.Where(x => x.PageNoteId == pageNote.Id).ToList();
            pageNote.Content = content;
            return Ok(pageNote);
        }

        // POST api/<PageNoteController>
        [HttpPost]
        public async Task<ActionResult<PageNote>> CreatePageNote([FromBody] PageNote pageNote)
        {
            if (ModelState.IsValid)
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
