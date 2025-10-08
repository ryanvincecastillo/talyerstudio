using Microsoft.EntityFrameworkCore;
using TalyerStudio.Inventory.Application.DTOs;
using TalyerStudio.Inventory.Application.Interfaces;
using TalyerStudio.Inventory.Domain.Entities;
using TalyerStudio.Inventory.Domain.Enums;
using TalyerStudio.Inventory.Infrastructure.Data;

namespace TalyerStudio.Inventory.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _context;

    public ProductRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.StockLevels)
            .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);
    }

    public async Task<List<Product>> GetAllAsync(Guid tenantId, Guid? categoryId = null, string? search = null, bool? isActive = null, int skip = 0, int take = 50)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.StockLevels)
            .Where(p => p.TenantId == tenantId && p.DeletedAt == null);

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(p => 
                p.Name.ToLower().Contains(search) ||
                p.Sku.ToLower().Contains(search) ||
                (p.Description != null && p.Description.ToLower().Contains(search)) ||
                (p.Brand != null && p.Brand.ToLower().Contains(search)));
        }

        if (isActive.HasValue)
        {
            query = query.Where(p => p.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(p => p.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<Product> CreateAsync(CreateProductDto dto, Guid tenantId)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Sku = dto.Sku,
            Name = dto.Name,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            ProductType = Enum.Parse<ProductType>(dto.ProductType.ToUpper()),
            Applicability = Enum.Parse<Applicability>(dto.Applicability.ToUpper()),
            Brand = dto.Brand,
            Model = dto.Model,
            PartNumber = dto.PartNumber,
            UnitPrice = dto.UnitPrice,
            CostPrice = dto.CostPrice,
            Unit = dto.Unit,
            ReorderLevel = dto.ReorderLevel,
            MaxStockLevel = dto.MaxStockLevel,
            SupplierName = dto.SupplierName,
            SupplierSku = dto.SupplierSku,
            Location = dto.Location,
            Barcode = dto.Barcode,
            Notes = dto.Notes,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);

        // Create initial stock level
        if (dto.InitialStock > 0)
        {
            var stockLevel = new StockLevel
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                BranchId = null, // Main warehouse
                CurrentQuantity = dto.InitialStock,
                ReservedQuantity = 0,
                LastRestockDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.StockLevels.Add(stockLevel);

            // Record stock movement
            var movement = new StockMovement
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                BranchId = null,
                MovementType = "IN",
                Quantity = dto.InitialStock,
                PreviousQuantity = 0,
                NewQuantity = dto.InitialStock,
                Reason = "Initial stock",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.StockMovements.Add(movement);
        }

        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null || product.DeletedAt != null)
            return false;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.CategoryId = dto.CategoryId;
        product.Brand = dto.Brand;
        product.Model = dto.Model;
        product.PartNumber = dto.PartNumber;
        product.UnitPrice = dto.UnitPrice;
        product.CostPrice = dto.CostPrice;
        product.ReorderLevel = dto.ReorderLevel;
        product.MaxStockLevel = dto.MaxStockLevel;
        product.SupplierName = dto.SupplierName;
        product.SupplierSku = dto.SupplierSku;
        product.Location = dto.Location;
        product.Barcode = dto.Barcode;
        product.Notes = dto.Notes;
        product.IsActive = dto.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null || product.DeletedAt != null)
            return false;

        product.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Product>> GetLowStockProductsAsync(Guid tenantId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.StockLevels)
            .Where(p => p.TenantId == tenantId && 
                        p.DeletedAt == null && 
                        p.IsActive &&
                        p.StockLevels.Sum(s => s.CurrentQuantity) <= p.ReorderLevel)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<bool> SkuExistsAsync(string sku, Guid tenantId, Guid? excludeProductId = null)
    {
        var query = _context.Products
            .Where(p => p.TenantId == tenantId && p.Sku == sku && p.DeletedAt == null);

        if (excludeProductId.HasValue)
        {
            query = query.Where(p => p.Id != excludeProductId.Value);
        }

        return await query.AnyAsync();
    }
}