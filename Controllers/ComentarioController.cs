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
        private readonly Comentario _comentario;

        public ComentarioController(RailwayContext _context, Comentario _comentario)
        {
             this._context = _context;
            this._comentario = _comentario;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comentario>> Get(int id)
        {

            try
            {  
            var comment = await _context.Comments.FindAsync(id);
                if(comment != null)
                {
                    return Ok(comment);
                }
                else
                {
                    return NotFound();
                    
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("comment")]
        public async Task<ActionResult<Comentario>> Post([FromBody]Comentario comentario)
        {
            try
            {
                    var comment = await _context.Comments.AddAsync(comentario);
                    return CreatedAtAction("Get", new { comentario.Id }, comentario);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<Comentario>> Update([FromBody]Comentario comentario)
        {
            try
            {
                if (_comentario.Post != null)
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

