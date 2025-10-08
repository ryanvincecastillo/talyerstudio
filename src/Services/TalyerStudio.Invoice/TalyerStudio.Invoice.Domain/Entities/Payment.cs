using TalyerStudio.Invoice.Domain.Enums;

namespace TalyerStudio.Invoice.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public string PaymentNumber { get; set; } = string.Empty;
    
    // Payment Details
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime PaymentDate { get; set; }
    
    // Additional Info
    public string? ReferenceNumber { get; set; }
    public string? Notes { get; set; }
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    
    // Navigation Property
    public Invoice Invoice { get; set; } = null!;
}