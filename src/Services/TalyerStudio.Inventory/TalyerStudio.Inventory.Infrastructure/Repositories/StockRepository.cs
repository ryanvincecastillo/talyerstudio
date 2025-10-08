using Microsoft.EntityFrameworkCore;
using TalyerStudio.Inventory.Application.DTOs;
using TalyerStudio.Inventory.Application.Interfaces;
using TalyerStudio.Inventory.Domain.Entities;
using TalyerStudio.Inventory.Infrastructure.Data;

namespace TalyerStudio.Inventory.Infrastructure.Repositories;

public class StockRepository : IStockRepository
{
    private readonly InventoryDbContext _context;

    public StockRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<StockLevel>> GetStockLevelsByProductIdAsync(Guid productId)
    {
        return await _context.StockLevels
            .Include(s => s.Product)
            .Where(s => s.ProductId == productId && s.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<StockLevel?> GetStockLevelAsync(Guid productId, Guid? branchId)
    {
        return await _context.StockLevels
            .FirstOrDefaultAsync(s => s.ProductId == productId && s.BranchId == branchId && s.DeletedAt == null);
    }

    public async Task<bool> AdjustStockAsync(Guid productId, AdjustStockDto dto, Guid? performedBy = null)
    {
        var stockLevel = await GetStockLevelAsync(productId, dto.BranchId);

        if (stockLevel == null)
        {
            // Create new stock level
            stockLevel = new StockLevel
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                BranchId = dto.BranchId,
                CurrentQuantity = 0,
                ReservedQuantity = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.StockLevels.Add(stockLevel);
        }

        var previousQuantity = stockLevel.CurrentQuantity;
        var quantityChange = dto.Quantity;

        // Adjust based on movement type
        switch (dto.MovementType.ToUpper())
        {
            case "IN":
                stockLevel.CurrentQuantity += quantityChange;
                stockLevel.LastRestockDate = DateTime.UtcNow;
                break;
            case "OUT":
                stockLevel.CurrentQuantity -= quantityChange;
                break;
            case "ADJUSTMENT":
                stockLevel.CurrentQuantity = quantityChange; // Direct set
                break;
            default:
                return false;
        }

        // Prevent negative stock
        if (stockLevel.CurrentQuantity < 0)
        {
            return false;
        }

        stockLevel.UpdatedAt = DateTime.UtcNow;

        // Record movement
        var movement = new StockMovement
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            BranchId = dto.BranchId,
            MovementType = dto.MovementType.ToUpper(),
            Quantity = dto.MovementType.ToUpper() == "ADJUSTMENT" ? quantityChange : quantityChange,
            PreviousQuantity = previousQuantity,
            NewQuantity = stockLevel.CurrentQuantity,
            Reference = dto.Reference,
            Reason = dto.Reason,
            Notes = dto.Notes,
            PerformedBy = performedBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.StockMovements.Add(movement);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<StockMovement>> GetStockMovementsAsync(Guid productId, int skip = 0, int take = 50)
    {
        return await _context.StockMovements
            .Where(m => m.ProductId == productId && m.DeletedAt == null)
            .OrderByDescending(m => m.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}