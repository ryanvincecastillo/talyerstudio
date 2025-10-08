namespace TalyerStudio.Customer.Domain.Entities;

public class Service
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    
    // Basic Info
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    
    // Pricing
    public decimal BasePrice { get; set; }
    public string Currency { get; set; } = "PHP";
    
    // Service Type
    public ServiceApplicability Applicability { get; set; } // AUTO, MOTORCYCLE, BOTH
    
    // Time Estimates
    public int? EstimatedDurationMinutes { get; set; }
    
    // Status
    public bool IsActive { get; set; } = true;
    
    // Display
    public int DisplayOrder { get; set; } = 0;
    public string? Icon { get; set; } // emoji or icon name
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    // Navigation
    public ServiceCategory? Category { get; set; }
}

public enum ServiceApplicability
{
    AUTO,
    MOTORCYCLE,
    BOTH
}