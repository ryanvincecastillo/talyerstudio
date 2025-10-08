using TalyerStudio.Customer.Domain.Entities;
using TalyerStudio.Shared.Contracts.Services;

namespace TalyerStudio.Customer.Application.Interfaces;

public interface IServiceCategoryRepository
{
    Task<ServiceCategory?> GetByIdAsync(Guid id);
    Task<List<ServiceCategory>> GetAllAsync(Guid tenantId, bool? isActive = null);
    Task<ServiceCategory> CreateAsync(CreateServiceCategoryDto dto, Guid tenantId);
    Task<bool> UpdateAsync(Guid id, UpdateServiceCategoryDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name, Guid tenantId, Guid? excludeCategoryId = null);
}