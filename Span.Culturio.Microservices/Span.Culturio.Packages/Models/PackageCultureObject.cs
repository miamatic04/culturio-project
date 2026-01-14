namespace Span.Culturio.Packages.Models
{
    public class PackageCultureObject
    {
        public int Id { get; set; }
        public int PackageId { get; set; }
        public int CultureObjectId { get; set; }
        public int AvailableVisits { get; set; }

        public Package Package { get; set; } = null!;
        public CultureObject CultureObject { get; set; } = null!;
    }
}