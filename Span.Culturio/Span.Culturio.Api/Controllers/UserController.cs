using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Span.Culturio.Api.Data;
using Span.Culturio.Api.DTOs.Users;

namespace Span.Culturio.Api.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly CulturioDbContext _context;

        public UsersController(CulturioDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            var totalCount = await _context.Users.CountAsync();

            var users = await _context.Users
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Username = u.Username
                })
                .ToListAsync();

            var response = new UsersPagedResponseDto
            {
                Users = users,
                TotalCount = totalCount
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var response = new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username
            };

            return Ok(response);
        }
    }
}
