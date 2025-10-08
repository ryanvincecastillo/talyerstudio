namespace TalyerStudio.Inventory.Application.DTOs;

public class AdjustStockDto
{
    public string MovementType { get; set; } = "ADJUSTMENT"; // IN, OUT, ADJUSTMENT
    public int Quantity { get; set; }
    public Guid? BranchId { get; set; }
    public string? Reference { get; set; }
    public string? Reason { get; set; }
    public string? Notes { get; set; }
}