using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Span.Culturio.Api.Data;
using Span.Culturio.Api.DTOs.Packages;

namespace Span.Culturio.Api.Controllers
{
    [ApiController]
    [Route("packages")]
    [Authorize]
    public class PackagesController : ControllerBase
    {
        private readonly CulturioDbContext _context;

        public PackagesController(CulturioDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPackages()
        {
            var packages = await _context.Packages
                .Include(p => p.PackageCultureObjects)
                .Select(p => new PackageResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    CultureObjects = p.PackageCultureObjects.Select(pco => new PackageCultureObjectDto
                    {
                        Id = pco.CultureObjectId,
                        AvailableVisits = pco.AvailableVisits
                    }).ToList(),
                    ValidDays = p.ValidDays
                })
                .ToListAsync();

            return Ok(packages);
        }
    }
}