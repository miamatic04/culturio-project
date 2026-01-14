using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Span.Culturio.Subscriptions.Data;
using Span.Culturio.Subscriptions.DTOs;
using Span.Culturio.Subscriptions.Models;

namespace Span.Culturio.Subscriptions.Controllers
{
    [ApiController]
    [Route("subscriptions")]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly SubscriptionsDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SubscriptionsController> _logger;

        public SubscriptionsController(
            SubscriptionsDbContext context,
            IHttpClientFactory httpClientFactory,
            ILogger<SubscriptionsController> logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionDto dto)
        {
            _logger.LogInformation("Creating subscription for User {UserId}, Package {PackageId}",
                dto.UserId, dto.PackageId);

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

            _logger.LogInformation("Subscription created with Id {SubscriptionId}", subscription.Id);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetSubscriptions([FromQuery] int? userId)
        {
            _logger.LogInformation("Getting subscriptions for UserId: {UserId}", userId);

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

            _logger.LogInformation("Returned {Count} subscriptions", subscriptions.Count);

            return Ok(subscriptions);
        }

        [HttpPost("track-visit")]
        public async Task<IActionResult> TrackVisit([FromBody] TrackVisitDto dto)
        {
            _logger.LogInformation("Tracking visit for Subscription {SubscriptionId}, CultureObject {CultureObjectId}",
                dto.SubscriptionId, dto.CultureObjectId);

            var subscription = await _context.Subscriptions.FindAsync(dto.SubscriptionId);

            if (subscription == null)
            {
                _logger.LogWarning("Subscription not found: {SubscriptionId}", dto.SubscriptionId);
                return NotFound("Subscription not found");
            }

            if (subscription.State != "active")
            {
                _logger.LogWarning("Subscription {SubscriptionId} is not active", dto.SubscriptionId);
                return BadRequest("Subscription is not active");
            }

            subscription.RecordedVisits++;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Visit tracked. Total visits: {RecordedVisits}", subscription.RecordedVisits);

            return Ok();
        }

        [HttpPost("activate")]
        public async Task<IActionResult> ActivateSubscription([FromBody] ActivateSubscriptionDto dto)
        {
            _logger.LogInformation("Activating subscription {SubscriptionId}", dto.SubscriptionId);

            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.Id == dto.SubscriptionId);

            if (subscription == null)
            {
                _logger.LogWarning("Subscription not found: {SubscriptionId}", dto.SubscriptionId);
                return NotFound("Subscription not found");
            }

            // Dohvati Package podatke iz Packages servisa
            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                // Kopirajte Authorization header iz trenutnog requesta
                if (Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", authHeader.ToString());
                }

                var packageResponse = await httpClient.GetAsync($"https://localhost:7005/packages/{subscription.PackageId}");

                if (!packageResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch package {PackageId} from Packages service", subscription.PackageId);
                    return BadRequest("Package not found");
                }

                var package = await packageResponse.Content.ReadFromJsonAsync<PackageDto>();

                // Aktiviraj subscription
                subscription.State = "active";
                subscription.ActiveFrom = DateTime.UtcNow;
                subscription.ActiveTo = DateTime.UtcNow.AddDays(package.ValidDays);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Subscription {SubscriptionId} activated until {ActiveTo}",
                    subscription.Id, subscription.ActiveTo);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating subscription {SubscriptionId}", dto.SubscriptionId);
                return StatusCode(500, "Error communicating with Packages service");
            }
        }
    }

    // DTO za Package response
    public class PackageDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ValidDays { get; set; }
    }
}