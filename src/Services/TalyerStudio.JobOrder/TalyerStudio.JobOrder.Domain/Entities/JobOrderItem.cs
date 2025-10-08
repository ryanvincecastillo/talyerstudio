namespace TalyerStudio.JobOrder.Domain.Entities;

public class JobOrderItem : BaseEntity
{
    public Guid JobOrderId { get; set; }
    public Guid ServiceId { get; set; }
    
    public string ServiceName { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    
    public string? Notes { get; set; }
    
    // Navigation Property
    public virtual JobOrder JobOrder { get; set; } = null!;
}