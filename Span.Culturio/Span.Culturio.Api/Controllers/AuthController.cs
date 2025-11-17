using Microsoft.AspNetCore.Mvc;
using Span.Culturio.Api.Data;
using Span.Culturio.Api.DTOs.Auth;
using Span.Culturio.Api.Models;

namespace Span.Culturio.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly CulturioDbContext _context;
        public AuthController(CulturioDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto dto)
        {
            // To be implemented
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            // To be implemented
            return Ok(new { token = "dummy-token" });
        }
    }
}
