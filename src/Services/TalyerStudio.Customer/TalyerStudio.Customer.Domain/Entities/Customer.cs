namespace TalyerStudio.Customer.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    
    // Address
    public string? Street { get; set; }
    public string? Barangay { get; set; }
    public string? Municipality { get; set; }
    public string? Province { get; set; }
    public string? ZipCode { get; set; }
    
    // Additional Info
    public DateTime? Birthday { get; set; }
    public int LoyaltyPoints { get; set; }
    public string CustomerType { get; set; } = "INDIVIDUAL"; // INDIVIDUAL or CORPORATE
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string? Notes { get; set; }
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
