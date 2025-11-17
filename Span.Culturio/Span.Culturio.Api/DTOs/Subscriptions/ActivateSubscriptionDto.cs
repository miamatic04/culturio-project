using System.ComponentModel.DataAnnotations;

namespace Span.Culturio.Api.DTOs.Subscriptions
{
    public class ActivateSubscriptionDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int SubscriptionId { get; set; }
    }
}