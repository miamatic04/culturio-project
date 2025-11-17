namespace Span.Culturio.Api.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ValidDays { get; set; }

        public ICollection<PackageCultureObject> PackageCultureObjects { get; set; } = new List<PackageCultureObject>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
