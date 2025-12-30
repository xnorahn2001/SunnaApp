using SnaaPlatform.Api.Models;

namespace SnaaPlatform.Api.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(Profile profile);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
