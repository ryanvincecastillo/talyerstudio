using Microsoft.EntityFrameworkCore;
using TalyerStudio.Customer.Application.Interfaces;
using TalyerStudio.Customer.Domain.Entities;
using TalyerStudio.Customer.Infrastructure.Data;
using TalyerStudio.Shared.Contracts.Services;

namespace TalyerStudio.Customer.Infrastructure.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly CustomerDbContext _context;

    public ServiceRepository(CustomerDbContext context)
    {
        _context = context;
    }

    public async Task<Service?> GetByIdAsync(Guid id)
    {
        return await _context.Services
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);
    }

    public async Task<List<Service>> GetAllAsync(Guid tenantId, Guid? categoryId = null, string? applicability = null, bool? isActive = null, int skip = 0, int take = 50)
    {
        var query = _context.Services
            .Include(s => s.Category)
            .Where(s => s.TenantId == tenantId && s.DeletedAt == null);

        if (categoryId.HasValue)
        {
            query = query.Where(s => s.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrEmpty(applicability))
        {
            if (Enum.TryParse<ServiceApplicability>(applicability, true, out var applicabilityEnum))
            {
                query = query.Where(s => s.Applicability == applicabilityEnum || s.Applicability == ServiceApplicability.BOTH);
            }
        }

        if (isActive.HasValue)
        {
            query = query.Where(s => s.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(s => s.DisplayOrder)
            .ThenBy(s => s.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<Service> CreateAsync(CreateServiceDto dto, Guid tenantId)
    {
        // Parse applicability enum
        if (!Enum.TryParse<ServiceApplicability>(dto.Applicability, true, out var applicability))
        {
            throw new ArgumentException("Invalid applicability. Valid values: AUTO, MOTORCYCLE, BOTH");
        }

        var service = new Service
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = dto.Name,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            BasePrice = dto.BasePrice,
            Currency = "PHP",
            Applicability = applicability,
            EstimatedDurationMinutes = dto.EstimatedDurationMinutes,
            IsActive = true,
            DisplayOrder = dto.DisplayOrder,
            Icon = dto.Icon,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        // Reload with category
        service = await _context.Services
            .Include(s => s.Category)
            .FirstAsync(s => s.Id == service.Id);

        return service;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateServiceDto dto)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (service == null)
        {
            return false;
        }

        // Parse applicability enum
        if (!Enum.TryParse<ServiceApplicability>(dto.Applicability, true, out var applicability))
        {
            throw new ArgumentException("Invalid applicability. Valid values: AUTO, MOTORCYCLE, BOTH");
        }

        service.Name = dto.Name;
        service.Description = dto.Description;
        service.CategoryId = dto.CategoryId;
        service.BasePrice = dto.BasePrice;
        service.Applicability = applicability;
        service.EstimatedDurationMinutes = dto.EstimatedDurationMinutes;
        service.IsActive = dto.IsActive;
        service.DisplayOrder = dto.DisplayOrder;
        service.Icon = dto.Icon;
        service.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (service == null)
        {
            return false;
        }

        service.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Service>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await _context.Services
            .Include(s => s.Category)
            .Where(s => s.CategoryId == categoryId && s.DeletedAt == null)
            .OrderBy(s => s.DisplayOrder)
            .ThenBy(s => s.Name)
            .ToListAsync();
    }
}