namespace TalyerStudio.JobOrder.Application.DTOs;

public class JobOrderItemDto
{
    public Guid Id { get; set; }
    public Guid JobOrderId { get; set; }
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    public string? Notes { get; set; }
}