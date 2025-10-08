namespace TalyerStudio.Shared.Contracts.Services;

public class ServiceCategoryDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ServiceCount { get; set; } // Number of services in this category
}

public class CreateServiceCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public string? Icon { get; set; }
    public string? Color { get; set; }
}

public class UpdateServiceCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public bool IsActive { get; set; } = true;
}

public class ServiceDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public decimal BasePrice { get; set; }
    public string Currency { get; set; } = "PHP";
    public string Applicability { get; set; } = string.Empty; // AUTO, MOTORCYCLE, BOTH
    public int? EstimatedDurationMinutes { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public string? Icon { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Computed
    public string FormattedPrice => $"₱{BasePrice:N2}";
    public string? EstimatedDuration => EstimatedDurationMinutes.HasValue 
        ? $"{EstimatedDurationMinutes / 60}h {EstimatedDurationMinutes % 60}m" 
        : null;
}

public class CreateServiceDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public decimal BasePrice { get; set; }
    public string Applicability { get; set; } = "BOTH"; // AUTO, MOTORCYCLE, BOTH
    public int? EstimatedDurationMinutes { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public string? Icon { get; set; }
}

public class UpdateServiceDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public decimal BasePrice { get; set; }
    public string Applicability { get; set; } = "BOTH";
    public int? EstimatedDurationMinutes { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; } = 0;
    public string? Icon { get; set; }
}