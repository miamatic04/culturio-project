using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Span.Culturio.Api.Data;
using Span.Culturio.Api.DTOs.Auth;
using Span.Culturio.Api.Models;
using Span.Culturio.Api.Services;
using System.Threading.Tasks;

namespace Span.Culturio.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly CulturioDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(
            CulturioDbContext context,
            IJwtService jwtService,
            ILogger<AuthController> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            _logger.LogInformation("Registering new user: {Username}", dto.Username);

            if(await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                _logger.LogWarning("Registration failed - username already exists: {Username}", dto.Username);
                return BadRequest(new { message = "Username already exists" });
            }
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                _logger.LogWarning("Registration failed - email already exists: {Email}", dto.Email);
                return BadRequest(new { message = "Email already exists" });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Username = dto.Username,
                Password = hashedPassword,
                Role = UserRole.User 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User registered successfully: {Username}", dto.Username);

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            _logger.LogInformation("Login attempt for user: {Username}", dto.Username);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null)
            {
                _logger.LogWarning("Login failed - user not found: {Username}", dto.Username);
                return BadRequest(new { message = "Invalid username or password" });
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                _logger.LogWarning("Login failed - invalid password for user: {Username}", dto.Username);
                return BadRequest(new { message = "Invalid username or password" });
            }

            var token = _jwtService.GenerateToken(user);

            _logger.LogInformation("User logged in successfully: {Username}", dto.Username);

            return Ok(new { token });
        }
    }
}
