using Span.Culturio.Auth.Models;

namespace Span.Culturio.Auth.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
