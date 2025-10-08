using TalyerStudio.Inventory.Application.DTOs;
using TalyerStudio.Inventory.Domain.Entities;

namespace TalyerStudio.Inventory.Application.Interfaces;

public interface IStockRepository
{
    Task<List<StockLevel>> GetStockLevelsByProductIdAsync(Guid productId);
    Task<StockLevel?> GetStockLevelAsync(Guid productId, Guid? branchId);
    Task<bool> AdjustStockAsync(Guid productId, AdjustStockDto dto, Guid? performedBy = null);
    Task<List<StockMovement>> GetStockMovementsAsync(Guid productId, int skip = 0, int take = 50);
}