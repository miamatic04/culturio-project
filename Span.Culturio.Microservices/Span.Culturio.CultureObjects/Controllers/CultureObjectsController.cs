using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Span.Culturio.CultureObjects.Data;
using Span.Culturio.CultureObjects.DTOs;
using Span.Culturio.CultureObjects.Models;

namespace Span.Culturio.CultureObjects.Controllers
{
    [ApiController]
    [Route("culture-objects")]
    [Authorize]
    public class CultureObjectsController : ControllerBase
    {
        private readonly CulturioDbContext _context;

        public CultureObjectsController(CulturioDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCultureObject([FromBody] CreateCultureObjectDto dto)
        {
            var cultureObject = new CultureObject
            {
                Name = dto.Name,
                ContactEmail = dto.ContactEmail,
                Address = dto.Address,
                ZipCode = dto.ZipCode,
                City = dto.City,
                AdminUserId = dto.AdminUserId
            };

            _context.CultureObjects.Add(cultureObject);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCultureObjects()
        {
            var cultureObjects = await _context.CultureObjects
                .Select(co => new CultureObjectResponseDto
                {
                    Id = co.Id,
                    Name = co.Name,
                    ContactEmail = co.ContactEmail,
                    ZipCode = co.ZipCode,
                    Address = co.Address,
                    City = co.City,
                    AdminUserId = co.AdminUserId
                })
                .ToListAsync();

            return Ok(cultureObjects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCultureObject(int id)
        {
            var cultureObject = await _context.CultureObjects.FindAsync(id);

            if (cultureObject == null)
            {
                return NotFound();
            }

            var response = new CultureObjectResponseDto
            {
                Id = cultureObject.Id,
                Name = cultureObject.Name,
                ContactEmail = cultureObject.ContactEmail,
                ZipCode = cultureObject.ZipCode,
                Address = cultureObject.Address,
                City = cultureObject.City,
                AdminUserId = cultureObject.AdminUserId
            };

            return Ok(response);
        }
    }
}
