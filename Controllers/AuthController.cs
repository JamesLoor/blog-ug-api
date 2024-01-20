using blog_ug_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog_ug_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        //private readonly IConfiguration _configuration;
        private readonly RailwayContext _context;

        public AuthController(RailwayContext context)
        {
            //_configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel user)
        {
            try
            {
                var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser == null)
                {
                    return Unauthorized(new { Message = "Credenciales inválidas" });
                }

                if (existingUser.Contrasena != user.Password)
                {
                    return Unauthorized(new { Message = "Credenciales inválidas" });
                }

                //retornar token jwt

                return Ok(new { Message = "Autenticación exitosa" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel user)
        {
            try
            {
                User newUser = new User();
                newUser.Nombre = user.Name;
                newUser.Email = user.Email;
                newUser.Contrasena = user.Password;
                //mascota.FechaCreacion = DateTime.Now;
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Registro exitoso" });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
