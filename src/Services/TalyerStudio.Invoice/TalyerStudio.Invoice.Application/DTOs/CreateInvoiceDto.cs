namespace TalyerStudio.Invoice.Application.DTOs;

public class CreateInvoiceDto
{
    public Guid TenantId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? JobOrderId { get; set; }
    
    public decimal TaxRate { get; set; } = 0.12m; // 12% VAT default for Philippines
    public decimal DiscountAmount { get; set; } = 0;
    
    public DateTime? DueDate { get; set; }
    public string? Notes { get; set; }
    public string? Terms { get; set; }
    
    public List<CreateInvoiceItemDto> Items { get; set; } = new();
}

public class CreateInvoiceItemDto
{
    public string ItemType { get; set; } = string.Empty; // SERVICE, PART, LABOR
    public Guid? ItemId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; } = 0;
}