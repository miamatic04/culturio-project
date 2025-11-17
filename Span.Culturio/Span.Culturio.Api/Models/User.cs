using Microsoft.AspNetCore.Components;

namespace Span.Culturio.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty ;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
