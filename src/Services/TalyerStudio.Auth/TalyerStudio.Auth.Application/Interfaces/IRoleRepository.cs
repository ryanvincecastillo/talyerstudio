using TalyerStudio.Auth.Domain.Entities;

namespace TalyerStudio.Auth.Application.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string name, Guid tenantId);
    Task<List<Role>> GetAllAsync(Guid tenantId);
}