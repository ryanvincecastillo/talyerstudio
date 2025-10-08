using TalyerStudio.Auth.Application.DTOs.Requests;
using TalyerStudio.Auth.Application.DTOs.Responses;

namespace TalyerStudio.Auth.Application.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request, string? ipAddress, string? userAgent);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken, string? ipAddress, string? userAgent);
    Task<bool> RevokeTokenAsync(string refreshToken);
    Task<UserDto?> GetUserByIdAsync(Guid userId);
}