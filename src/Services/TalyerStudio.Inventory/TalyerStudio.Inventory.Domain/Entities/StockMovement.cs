namespace TalyerStudio.Inventory.Domain.Entities;

public class StockMovement : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid? BranchId { get; set; }
    
    public string MovementType { get; set; } = string.Empty; // IN, OUT, ADJUSTMENT, TRANSFER
    public int Quantity { get; set; }
    public int PreviousQuantity { get; set; }
    public int NewQuantity { get; set; }
    
    public string? Reference { get; set; } // Job Order ID, Purchase Order ID, etc.
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    
    public Guid? PerformedBy { get; set; } // User ID
}