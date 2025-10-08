using TalyerStudio.Auth.Domain.Entities;

namespace TalyerStudio.Auth.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<RefreshToken> CreateAsync(RefreshToken refreshToken);
    Task UpdateAsync(RefreshToken refreshToken);
    Task SaveChangesAsync();
}