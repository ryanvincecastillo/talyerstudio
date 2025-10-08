using TalyerStudio.Inventory.Application.DTOs;
using TalyerStudio.Inventory.Application.Interfaces;
using TalyerStudio.Inventory.Domain.Entities;

namespace TalyerStudio.Inventory.Application.Services;

public class StockService : IStockService
{
    private readonly IStockRepository _repository;

    public StockService(IStockRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<StockLevelDto>> GetStockLevelsByProductIdAsync(Guid productId)
    {
        var stockLevels = await _repository.GetStockLevelsByProductIdAsync(productId);
        return stockLevels.Select(MapToDto).ToList();
    }

    public async Task<bool> AdjustStockAsync(Guid productId, AdjustStockDto dto, Guid? performedBy = null)
    {
        return await _repository.AdjustStockAsync(productId, dto, performedBy);
    }

    private StockLevelDto MapToDto(StockLevel stockLevel)
    {
        return new StockLevelDto
        {
            Id = stockLevel.Id,
            ProductId = stockLevel.ProductId,
            ProductName = stockLevel.Product?.Name ?? string.Empty,
            ProductSku = stockLevel.Product?.Sku ?? string.Empty,
            BranchId = stockLevel.BranchId,
            BranchName = null, // TODO: Get branch name when branch service exists
            CurrentQuantity = stockLevel.CurrentQuantity,
            ReservedQuantity = stockLevel.ReservedQuantity,
            AvailableQuantity = stockLevel.AvailableQuantity,
            LastRestockDate = stockLevel.LastRestockDate,
            LastCountDate = stockLevel.LastCountDate
        };
    }
}