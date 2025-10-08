using TalyerStudio.Auth.Domain.Entities;

namespace TalyerStudio.Auth.Application.Services;

public interface IJwtService
{
    string GenerateAccessToken(User user, List<string> roles, List<string> permissions);
    string GenerateRefreshToken();
    Guid? ValidateAccessToken(string token);
}