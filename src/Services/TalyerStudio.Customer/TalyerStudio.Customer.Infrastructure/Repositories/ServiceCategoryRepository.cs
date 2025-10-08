using Microsoft.EntityFrameworkCore;
using TalyerStudio.Customer.Application.Interfaces;
using TalyerStudio.Customer.Domain.Entities;
using TalyerStudio.Customer.Infrastructure.Data;
using TalyerStudio.Shared.Contracts.Services;

namespace TalyerStudio.Customer.Infrastructure.Repositories;

public class ServiceCategoryRepository : IServiceCategoryRepository
{
    private readonly CustomerDbContext _context;

    public ServiceCategoryRepository(CustomerDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceCategory?> GetByIdAsync(Guid id)
    {
        return await _context.ServiceCategories
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
    }

    public async Task<List<ServiceCategory>> GetAllAsync(Guid tenantId, bool? isActive = null)
    {
        var query = _context.ServiceCategories
            .Where(c => c.TenantId == tenantId && c.DeletedAt == null);

        if (isActive.HasValue)
        {
            query = query.Where(c => c.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(c => c.DisplayOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<ServiceCategory> CreateAsync(CreateServiceCategoryDto dto, Guid tenantId)
    {
        var category = new ServiceCategory
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = dto.Name,
            Description = dto.Description,
            Icon = dto.Icon,
            IsActive = true,
            DisplayOrder = dto.DisplayOrder,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.ServiceCategories.Add(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateServiceCategoryDto dto)
    {
        var category = await _context.ServiceCategories
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (category == null)
        {
            return false;
        }

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.Icon = dto.Icon;
        category.IsActive = dto.IsActive;
        category.DisplayOrder = dto.DisplayOrder;
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await _context.ServiceCategories
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (category == null)
        {
            return false;
        }

        category.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid tenantId, Guid? excludeCategoryId = null)
    {
        var query = _context.ServiceCategories
            .Where(c => c.TenantId == tenantId && c.Name == name && c.DeletedAt == null);

        if (excludeCategoryId.HasValue)
        {
            query = query.Where(c => c.Id != excludeCategoryId.Value);
        }

        return await query.AnyAsync();
    }
}