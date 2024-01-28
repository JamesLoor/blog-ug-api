using blog_ug_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;
using System.Net;

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

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    return NotFound(new { Message = "No se encontró el usuario con este correo electrónico." });
                }


                EnviarCorreo(user.Email);

                return Ok(new { Message = "Se ha enviado un correo con el enlace para restablecer la contraseña." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("complete-reset")]
        public async Task<IActionResult> CompleteReset([FromBody] CompleteResetPasswordModel model)
        {
            try
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (existingUser == null)
                {
                    return NotFound(new { Message = "Token inválido" });
                }

                existingUser.Contrasena = model.NewPassword;

                await _context.SaveChangesAsync();

                return Ok(new { Message = "Contraseña restablecida exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void EnviarCorreo(string email)
        {
            MailAddress addressFrom = new MailAddress("blog.ug.proyecto@gmail.com", "Blog UG");
            MailAddress addressTo = new MailAddress(email);
            MailMessage message = new MailMessage(addressFrom, addressTo);

            message.IsBodyHtml = true;
            message.Body = GenerarCodigoAleatorio();
            message.Subject = "Recuperación de contraseña";

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("blog.ug.proyecto@gmail.com", "cvrg mkar sphm wegy");


            smtpClient.Send(message);
        }
        private string GenerarCodigoAleatorio()
        {
            var random = new Random();
            var codigo = "";

            for (int i = 0; i < 6; i++)
            {
                codigo += random.Next(0, 10).ToString();
            }

            return codigo;
        }
    }
}
