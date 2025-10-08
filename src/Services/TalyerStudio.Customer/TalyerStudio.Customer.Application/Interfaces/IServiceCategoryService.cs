using TalyerStudio.Shared.Contracts.Services;

namespace TalyerStudio.Customer.Application.Interfaces;

public interface IServiceCategoryService
{
    Task<ServiceCategoryDto?> GetByIdAsync(Guid id);
    Task<List<ServiceCategoryDto>> GetAllAsync(Guid tenantId, bool? isActive = null);
    Task<ServiceCategoryDto> CreateAsync(CreateServiceCategoryDto dto, Guid tenantId);
    Task<bool> UpdateAsync(Guid id, UpdateServiceCategoryDto dto);
    Task<bool> DeleteAsync(Guid id);
}