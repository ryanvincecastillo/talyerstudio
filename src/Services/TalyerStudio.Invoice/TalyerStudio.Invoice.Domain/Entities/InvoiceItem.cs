namespace TalyerStudio.Invoice.Domain.Entities;

public class InvoiceItem
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    
    // Item Details
    public string ItemType { get; set; } = string.Empty; // SERVICE, PART, LABOR
    public Guid? ItemId { get; set; } // Reference to Service or Product
    public string Description { get; set; } = string.Empty;
    
    // Pricing
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    
    // Navigation Property
    public Invoice Invoice { get; set; } = null!;
}