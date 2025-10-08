using TalyerStudio.Auth.Domain.Entities;

namespace TalyerStudio.Auth.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email, Guid tenantId);
    Task<User?> GetByEmailWithRolesAndPermissionsAsync(string email);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task SaveChangesAsync();
}