using TalyerStudio.Inventory.Application.DTOs;
using TalyerStudio.Inventory.Application.Interfaces;
using TalyerStudio.Inventory.Domain.Entities;

namespace TalyerStudio.Inventory.Application.Services;

public class ProductCategoryService : IProductCategoryService
{
    private readonly IProductCategoryRepository _repository;

    public ProductCategoryService(IProductCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductCategoryDto?> GetByIdAsync(Guid id)
    {
        var category = await _repository.GetByIdAsync(id);
        return category == null ? null : MapToDto(category);
    }

    public async Task<List<ProductCategoryDto>> GetAllAsync(Guid tenantId, bool? isActive = null)
    {
        var categories = await _repository.GetAllAsync(tenantId, isActive);
        return categories.Select(MapToDto).ToList();
    }

    public async Task<ProductCategoryDto> CreateAsync(CreateProductCategoryDto dto, Guid tenantId)
    {
        var category = await _repository.CreateAsync(dto, tenantId);
        return MapToDto(category);
    }

    public async Task<bool> UpdateAsync(Guid id, CreateProductCategoryDto dto)
    {
        return await _repository.UpdateAsync(id, dto);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private ProductCategoryDto MapToDto(ProductCategory category)
    {
        return new ProductCategoryDto
        {
            Id = category.Id,
            TenantId = category.TenantId,
            Name = category.Name,
            Description = category.Description,
            Icon = category.Icon,
            Color = category.Color,
            DisplayOrder = category.DisplayOrder,
            IsActive = category.IsActive,
            ParentCategoryId = category.ParentCategoryId,
            ParentCategoryName = category.ParentCategory?.Name,
            ProductCount = category.Products?.Count(p => p.DeletedAt == null) ?? 0,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
}