using TalyerStudio.Customer.Domain.Entities;
using TalyerStudio.Shared.Contracts.Services;

namespace TalyerStudio.Customer.Application.Interfaces;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(Guid id);
    Task<List<Service>> GetAllAsync(Guid tenantId, Guid? categoryId = null, string? applicability = null, bool? isActive = null, int skip = 0, int take = 50);
    Task<Service> CreateAsync(CreateServiceDto dto, Guid tenantId);
    Task<bool> UpdateAsync(Guid id, UpdateServiceDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<List<Service>> GetByCategoryIdAsync(Guid categoryId);
}