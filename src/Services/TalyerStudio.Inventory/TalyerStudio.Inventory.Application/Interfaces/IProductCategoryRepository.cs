using TalyerStudio.Inventory.Application.DTOs;
using TalyerStudio.Inventory.Domain.Entities;

namespace TalyerStudio.Inventory.Application.Interfaces;

public interface IProductCategoryRepository
{
    Task<ProductCategory?> GetByIdAsync(Guid id);
    Task<List<ProductCategory>> GetAllAsync(Guid tenantId, bool? isActive = null);
    Task<ProductCategory> CreateAsync(CreateProductCategoryDto dto, Guid tenantId);
    Task<bool> UpdateAsync(Guid id, CreateProductCategoryDto dto);
    Task<bool> DeleteAsync(Guid id);
}