using TalyerStudio.Shared.Contracts.Services;

namespace TalyerStudio.Customer.Application.Interfaces;

public interface IServiceService
{
    Task<ServiceDto?> GetByIdAsync(Guid id);
    Task<List<ServiceDto>> GetAllAsync(Guid tenantId, Guid? categoryId = null, string? applicability = null, bool? isActive = null, int skip = 0, int take = 50);
    Task<ServiceDto> CreateAsync(CreateServiceDto dto, Guid tenantId);
    Task<bool> UpdateAsync(Guid id, UpdateServiceDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<List<ServiceDto>> GetByCategoryIdAsync(Guid categoryId);
}