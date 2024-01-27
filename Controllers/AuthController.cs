using blog_ug_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace blog_ug_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly RailwayContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(RailwayContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

                var token = GenerateJwtToken(existingUser);

                return Ok(new { Message = "Autenticación exitosa", Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
            };

            var jwtKey = _configuration["Jwt:Key"];
            if (jwtKey == null)
            {
                throw new ArgumentException("La clave JWT no puede ser nula.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha384);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "El correo electrónico ya está registrado.");
                    return BadRequest(ModelState);
                }

                var newUser = new User
                {
                    Nombre = user.Name,
                    Email = user.Email,
                    Contrasena = user.Password,
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Registro exitoso" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
