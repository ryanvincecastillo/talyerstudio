using TalyerStudio.Inventory.Application.DTOs;
using TalyerStudio.Inventory.Application.Interfaces;
using TalyerStudio.Inventory.Domain.Entities;

namespace TalyerStudio.Inventory.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product == null ? null : MapToDto(product);
    }

    public async Task<List<ProductDto>> GetAllAsync(Guid tenantId, Guid? categoryId = null, string? search = null, bool? isActive = null, int skip = 0, int take = 50)
    {
        var products = await _repository.GetAllAsync(tenantId, categoryId, search, isActive, skip, take);
        return products.Select(MapToDto).ToList();
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto, Guid tenantId)
    {
        // Check if SKU exists
        var skuExists = await _repository.SkuExistsAsync(dto.Sku, tenantId);
        if (skuExists)
        {
            throw new InvalidOperationException($"Product with SKU '{dto.Sku}' already exists");
        }

        var product = await _repository.CreateAsync(dto, tenantId);
        return MapToDto(product);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        return await _repository.UpdateAsync(id, dto);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<List<ProductDto>> GetLowStockProductsAsync(Guid tenantId)
    {
        var products = await _repository.GetLowStockProductsAsync(tenantId);
        return products.Select(MapToDto).ToList();
    }

    private ProductDto MapToDto(Product product)
    {
        var totalStock = product.StockLevels.Sum(s => s.CurrentQuantity);
        var availableStock = product.StockLevels.Sum(s => s.AvailableQuantity);

        return new ProductDto
        {
            Id = product.Id,
            TenantId = product.TenantId,
            Sku = product.Sku,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
            ProductType = product.ProductType.ToString(),
            Applicability = product.Applicability.ToString(),
            Brand = product.Brand,
            Model = product.Model,
            PartNumber = product.PartNumber,
            UnitPrice = product.UnitPrice,
            CostPrice = product.CostPrice,
            Currency = product.Currency,
            Unit = product.Unit,
            ReorderLevel = product.ReorderLevel,
            MaxStockLevel = product.MaxStockLevel,
            SupplierName = product.SupplierName,
            SupplierSku = product.SupplierSku,
            Location = product.Location,
            Barcode = product.Barcode,
            Notes = product.Notes,
            IsActive = product.IsActive,
            Images = product.Images,
            TotalStock = totalStock,
            AvailableStock = availableStock,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}