namespace Span.Culturio.Api.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PackageId { get; set; }  
        public string Name { get; set; } = string.Empty;
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public string State { get; set; } = "inactive";
        public int RecordedVisits { get; set; }

        public User User { get; set; } = null!;
        public Package Package { get; set; } = null!;
    }
}
