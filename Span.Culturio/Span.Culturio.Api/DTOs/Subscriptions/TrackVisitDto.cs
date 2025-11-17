using System.ComponentModel.DataAnnotations;

namespace Span.Culturio.Api.DTOs.Subscriptions
{
    public class TrackVisitDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int SubscriptionId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CultureObjectId { get; set; }
    }
}