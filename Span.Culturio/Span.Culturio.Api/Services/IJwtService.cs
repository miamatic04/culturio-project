using Span.Culturio.Api.Models;

namespace Span.Culturio.Api.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
