namespace TalyerStudio.Customer.Domain.Entities;

public class ServiceCategory
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    
    // Basic Info
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // Display
    public int DisplayOrder { get; set; } = 0;
    public string? Icon { get; set; }
    public string? Color { get; set; } // hex color for UI
    
    // Status
    public bool IsActive { get; set; } = true;
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    // Navigation
    public ICollection<Service> Services { get; set; } = new List<Service>();
}