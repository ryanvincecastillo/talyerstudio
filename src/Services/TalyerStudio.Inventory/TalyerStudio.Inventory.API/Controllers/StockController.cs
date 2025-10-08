using Microsoft.AspNetCore.Mvc;
using TalyerStudio.Inventory.Application.DTOs;
using TalyerStudio.Inventory.Application.Interfaces;

namespace TalyerStudio.Inventory.API.Controllers;

[ApiController]
[Route("api/stock")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly ILogger<StockController> _logger;

    public StockController(IStockService stockService, ILogger<StockController> logger)
    {
        _stockService = stockService;
        _logger = logger;
    }

    /// <summary>
    /// Get stock levels for a product
    /// </summary>
    [HttpGet("product/{productId}")]
    public async Task<ActionResult<List<StockLevelDto>>> GetStockLevels(Guid productId)
    {
        try
        {
            var stockLevels = await _stockService.GetStockLevelsByProductIdAsync(productId);
            return Ok(stockLevels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting stock levels for product {ProductId}", productId);
            return StatusCode(500, "An error occurred while retrieving stock levels");
        }
    }

    /// <summary>
    /// Adjust stock for a product
    /// </summary>
    [HttpPost("product/{productId}/adjust")]
    public async Task<ActionResult> AdjustStock(Guid productId, [FromBody] AdjustStockDto dto)
    {
        try
        {
            var success = await _stockService.AdjustStockAsync(productId, dto, null);
            
            if (!success)
                return BadRequest("Failed to adjust stock. Check quantity and movement type.");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adjusting stock for product {ProductId}", productId);
            return StatusCode(500, "An error occurred while adjusting stock");
        }
    }
}