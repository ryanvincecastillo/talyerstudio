using Microsoft.EntityFrameworkCore;
using TalyerStudio.Inventory.Application.DTOs;
using TalyerStudio.Inventory.Application.Interfaces;
using TalyerStudio.Inventory.Domain.Entities;
using TalyerStudio.Inventory.Infrastructure.Data;

namespace TalyerStudio.Inventory.Infrastructure.Repositories;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly InventoryDbContext _context;

    public ProductCategoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<ProductCategory?> GetByIdAsync(Guid id)
    {
        return await _context.ProductCategories
            .Include(c => c.ParentCategory)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
    }

    public async Task<List<ProductCategory>> GetAllAsync(Guid tenantId, bool? isActive = null)
    {
        var query = _context.ProductCategories
            .Include(c => c.ParentCategory)
            .Include(c => c.Products)
            .Where(c => c.TenantId == tenantId && c.DeletedAt == null);

        if (isActive.HasValue)
        {
            query = query.Where(c => c.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(c => c.DisplayOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<ProductCategory> CreateAsync(CreateProductCategoryDto dto, Guid tenantId)
    {
        var category = new ProductCategory
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = dto.Name,
            Description = dto.Description,
            Icon = dto.Icon,
            Color = dto.Color,
            DisplayOrder = dto.DisplayOrder,
            ParentCategoryId = dto.ParentCategoryId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.ProductCategories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<bool> UpdateAsync(Guid id, CreateProductCategoryDto dto)
    {
        var category = await _context.ProductCategories.FindAsync(id);
        if (category == null || category.DeletedAt != null)
            return false;

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.Icon = dto.Icon;
        category.Color = dto.Color;
        category.DisplayOrder = dto.DisplayOrder;
        category.ParentCategoryId = dto.ParentCategoryId;
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await _context.ProductCategories.FindAsync(id);
        if (category == null || category.DeletedAt != null)
            return false;

        // Check if category has products
        var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id && p.DeletedAt == null);
        if (hasProducts)
            return false; // Cannot delete category with products

        category.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}