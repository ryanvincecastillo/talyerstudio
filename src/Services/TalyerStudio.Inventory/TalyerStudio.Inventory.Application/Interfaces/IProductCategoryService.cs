using TalyerStudio.Inventory.Application.DTOs;

namespace TalyerStudio.Inventory.Application.Interfaces;

public interface IProductCategoryService
{
    Task<ProductCategoryDto?> GetByIdAsync(Guid id);
    Task<List<ProductCategoryDto>> GetAllAsync(Guid tenantId, bool? isActive = null);
    Task<ProductCategoryDto> CreateAsync(CreateProductCategoryDto dto, Guid tenantId);
    Task<bool> UpdateAsync(Guid id, CreateProductCategoryDto dto);
    Task<bool> DeleteAsync(Guid id);
}