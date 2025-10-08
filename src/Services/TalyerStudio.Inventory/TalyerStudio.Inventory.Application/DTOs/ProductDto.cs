namespace TalyerStudio.Inventory.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    
    public string ProductType { get; set; } = string.Empty;
    public string Applicability { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? PartNumber { get; set; }
    
    public decimal UnitPrice { get; set; }
    public decimal? CostPrice { get; set; }
    public string Currency { get; set; } = "PHP";
    
    public string Unit { get; set; } = "pcs";
    public int ReorderLevel { get; set; }
    public int? MaxStockLevel { get; set; }
    
    public string? SupplierName { get; set; }
    public string? SupplierSku { get; set; }
    
    public string? Location { get; set; }
    public string? Barcode { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    
    public string[] Images { get; set; } = Array.Empty<string>();
    
    // Stock info
    public int TotalStock { get; set; }
    public int AvailableStock { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}