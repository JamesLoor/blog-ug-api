using blog_ug_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace blog_ug_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginViewModel user)
        {
            return Ok(new { Email = user.Email, Password = user.Password });
        }
    }
}
