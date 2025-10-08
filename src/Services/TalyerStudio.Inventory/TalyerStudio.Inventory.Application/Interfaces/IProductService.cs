using TalyerStudio.Inventory.Application.DTOs;

namespace TalyerStudio.Inventory.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<List<ProductDto>> GetAllAsync(Guid tenantId, Guid? categoryId = null, string? search = null, bool? isActive = null, int skip = 0, int take = 50);
    Task<ProductDto> CreateAsync(CreateProductDto dto, Guid tenantId);
    Task<bool> UpdateAsync(Guid id, UpdateProductDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<List<ProductDto>> GetLowStockProductsAsync(Guid tenantId);
}