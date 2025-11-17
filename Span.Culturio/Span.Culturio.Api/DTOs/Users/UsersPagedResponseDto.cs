namespace Span.Culturio.Api.DTOs.Users
{
    public class UsersPagedResponseDto
    {
        public List<UserResponseDto> Users { get; set; } = new List<UserResponseDto>();
        public int TotalCount { get; set; }
    }
}
