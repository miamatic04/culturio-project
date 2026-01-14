using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Span.Culturio.Api.Data;
using Span.Culturio.Api.DTOs.Subscriptions;
using Span.Culturio.Api.Models;

namespace Span.Culturio.Api.Controllers
{
    [ApiController]
    [Route("subscriptions")]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly CulturioDbContext _context;

        public SubscriptionsController(CulturioDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionDto dto)
        {
            var subscription = new Subscription
            {
                UserId = dto.UserId,
                PackageId = dto.PackageId,
                Name = dto.Name,
                State = "inactive",
                RecordedVisits = 0
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetSubscriptions([FromQuery] int? userId)
        {
            var query = _context.Subscriptions.AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(s => s.UserId == userId.Value);
            }

            var subscriptions = await query
                .Select(s => new SubscriptionResponseDto
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    PackageId = s.PackageId,
                    Name = s.Name,
                    ActiveFrom = s.ActiveFrom,
                    ActiveTo = s.ActiveTo,
                    State = s.State,
                    RecordedVisits = s.RecordedVisits
                })
                .ToListAsync();

            return Ok(subscriptions);
        }

        [HttpPost("track-visit")]
        public async Task<IActionResult> TrackVisit([FromBody] TrackVisitDto dto)
        {
            var subscription = await _context.Subscriptions.FindAsync(dto.SubscriptionId);

            if (subscription == null)
            {
                return NotFound("Subscription not found");
            }

            if (subscription.State != "active")
            {
                return BadRequest("Subscription is not active");
            }

            subscription.RecordedVisits++;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("activate")]
        public async Task<IActionResult> ActivateSubscription([FromBody] ActivateSubscriptionDto dto)
        {
            var subscription = await _context.Subscriptions
                .Include(s => s.Package)
                .FirstOrDefaultAsync(s => s.Id == dto.SubscriptionId);

            if (subscription == null)
            {
                return NotFound("Subscription not found");
            }

            // Aktiviraj subscription
            subscription.State = "active";
            subscription.ActiveFrom = DateTime.UtcNow;
            subscription.ActiveTo = DateTime.UtcNow.AddDays(subscription.Package.ValidDays);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}