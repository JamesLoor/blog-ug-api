using blog_ug_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog_ug_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly RailwayContext _context;

        public PostController(RailwayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> Get()
        {
            try
            {
                var posts = await _context.Posts
                    .Include(p => p.Categorias)
                    .ToListAsync();

                if (posts == null || !posts.Any())
                {
                    return NotFound(new { Message = "No se encontraron blogs" });
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetById(int id)
        {
            try
            {
                var post = await _context.Posts
                    .Include(p => p.Categorias)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (post == null)
                {
                    return NotFound(new { Message = $"No se encontró el blog con el ID: {id}" });
                }

                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            if (post == null)
            {
                return BadRequest("El post no puede ser nulo.");
            }

            try
            {
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Get", new { id = post.Id }, post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        [Authorize]
        public void Delete(int id)
        {
        }
    }
}

