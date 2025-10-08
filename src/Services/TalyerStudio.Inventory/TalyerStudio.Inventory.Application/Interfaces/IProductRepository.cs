using TalyerStudio.Inventory.Application.DTOs;
using TalyerStudio.Inventory.Domain.Entities;

namespace TalyerStudio.Inventory.Application.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> GetAllAsync(Guid tenantId, Guid? categoryId = null, string? search = null, bool? isActive = null, int skip = 0, int take = 50);
    Task<Product> CreateAsync(CreateProductDto dto, Guid tenantId);
    Task<bool> UpdateAsync(Guid id, UpdateProductDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<List<Product>> GetLowStockProductsAsync(Guid tenantId);
    Task<bool> SkuExistsAsync(string sku, Guid tenantId, Guid? excludeProductId = null);
}