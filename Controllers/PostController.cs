﻿using System.Security.Claims;
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
                    .Select(p => new
                    {
                        p.Id,
                        p.Imagen,
                        p.Titulo,
                        p.Descripcion,
                        p.Contenido,
                        p.Comentarios,
                        Usuario = new
                        {
                            p.Usuario.Id,
                            p.Usuario.Nombre,
                            p.Usuario.Email
                        },
                        p.Categorias,
                        p.FechaPublicacion
                    })
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
                    .Select(p => new
                    {
                        p.Id,
                        p.Imagen,
                        p.Titulo,
                        p.Descripcion,
                        p.Contenido,
                        p.Comentarios,
                        Usuario = new
                        {
                            p.Usuario.Id,
                            p.Usuario.Nombre,
                            p.Usuario.Email
                        },
                        p.Categorias
                    })
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
        public async Task<IActionResult> Create([FromBody] PostViewModel post)
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    var newPost = new Post
                    {
                        Titulo = post.Titulo,
                        Categorias = post.Categorias,
                        Contenido = post.Contenido,
                        Descripcion = post.Descripcion,
                        UsuarioId = int.Parse(userId),
                        Imagen = post.Imagen,
                    };

                    _context.Posts.Add(newPost);
                    await _context.SaveChangesAsync();
                    return Ok("Post creado con éxito");
                }
                return Unauthorized("Usuario no autenticado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] PostViewModel post)
        {
            try
            {
                var existingPost = await _context.Posts.FindAsync(id);

                if (existingPost == null)
                {
                    return NotFound("Post no encontrado");  
                }

                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null || existingPost.UsuarioId != int.Parse(userId))
                {
                    return Unauthorized("Operación no autorizada");
                }

                existingPost.Titulo = post.Titulo;
                existingPost.Descripcion = post.Descripcion;
                existingPost.Contenido = post.Contenido;
                existingPost.Imagen = post.Imagen;
                existingPost.Categorias = post.Categorias;

                await _context.SaveChangesAsync();

                return Ok("Post actualizado con éxito");
            }   
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var post = await _context.Posts.FindAsync(id);

                if (post == null)
                {
                    return NotFound("Post no encontrado");
                }

                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null || post.UsuarioId != int.Parse(userId))
                {
                    return Unauthorized("Operación no autorizada");
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                return Ok("Post eliminado con éxito");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

