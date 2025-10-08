namespace TalyerStudio.Inventory.Domain.Entities;

public class StockLevel : BaseEntity
{
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
    
    public Guid? BranchId { get; set; } // null = main warehouse
    
    public int CurrentQuantity { get; set; } = 0;
    public int ReservedQuantity { get; set; } = 0; // For pending orders
    public int AvailableQuantity => CurrentQuantity - ReservedQuantity;
    
    public DateTime? LastRestockDate { get; set; }
    public DateTime? LastCountDate { get; set; }
}