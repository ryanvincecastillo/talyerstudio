using TalyerStudio.Inventory.Application.DTOs;

namespace TalyerStudio.Inventory.Application.Interfaces;

public interface IStockService
{
    Task<List<StockLevelDto>> GetStockLevelsByProductIdAsync(Guid productId);
    Task<bool> AdjustStockAsync(Guid productId, AdjustStockDto dto, Guid? performedBy = null);
}