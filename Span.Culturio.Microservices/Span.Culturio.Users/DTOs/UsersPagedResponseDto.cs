namespace Span.Culturio.Users.DTOs
{
    public class UsersPagedResponseDto
    {
        public List<UserResponseDto> Users { get; set; } = new List<UserResponseDto>();
        public int TotalCount { get; set; }
    }
}
