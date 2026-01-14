namespace Span.Culturio.Packages.DTOs
{
    public class PackageResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<PackageCultureObjectDto> CultureObjects { get; set; } = new List<PackageCultureObjectDto>();
        public int ValidDays { get; set; }
    }

    public class PackageCultureObjectDto
    {
        public int Id { get; set; }
        public int AvailableVisits { get; set; }
    }
}