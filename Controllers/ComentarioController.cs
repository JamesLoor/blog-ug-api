using Microsoft.AspNetCore.Mvc;
using blog_ug_api.Models;
using Microsoft.EntityFrameworkCore;

namespace blog_ug_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComentarioController : ControllerBase
    {
        private readonly RailwayContext _context;
        Comentario _comentario;
        public ComentarioController(RailwayContext context)
        {
            RailwayContext _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {  
            var comment = await _context.Comments.FindAsync(id);
                if(comment != null)
                {
                    return Ok(comment);
                }else
                {
                    return NoContent();
                }
           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("comment")]
        public async Task<ActionResult> Post(Comentario _comentario)
        {
            try
            {
                var comments = await _context.Comments.AddAsync(_comentario);
                return CreatedAtAction("Get", new { id = _comentario.Id }, _comentario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update(Comentario comentario)
        {
            try
            {
                if (_comentario.PostId != 0)
                {
                    _comentario.Contenido = comentario.Contenido;
                    await _context.SaveChangesAsync();
                    return Ok(_comentario);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }

}

