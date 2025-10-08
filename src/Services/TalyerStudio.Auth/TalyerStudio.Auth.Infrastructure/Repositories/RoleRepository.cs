using Microsoft.EntityFrameworkCore;
using TalyerStudio.Auth.Application.Interfaces;
using TalyerStudio.Auth.Domain.Entities;
using TalyerStudio.Auth.Infrastructure.Data;

namespace TalyerStudio.Auth.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AuthDbContext _context;

    public RoleRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByNameAsync(string name, Guid tenantId)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name && r.TenantId == tenantId);
    }

    public async Task<List<Role>> GetAllAsync(Guid tenantId)
    {
        return await _context.Roles
            .Where(r => r.TenantId == tenantId)
            .ToListAsync();
    }
}