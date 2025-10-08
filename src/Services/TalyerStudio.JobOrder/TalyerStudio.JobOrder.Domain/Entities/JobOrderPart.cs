namespace TalyerStudio.JobOrder.Domain.Entities;

public class JobOrderPart : BaseEntity
{
    public Guid JobOrderId { get; set; }
    public Guid ProductId { get; set; }
    
    public string ProductName { get; set; } = string.Empty;
    public string? ProductSku { get; set; }
    
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    
    public string? Notes { get; set; }
    
    // Navigation Property
    public virtual JobOrder JobOrder { get; set; } = null!;
}