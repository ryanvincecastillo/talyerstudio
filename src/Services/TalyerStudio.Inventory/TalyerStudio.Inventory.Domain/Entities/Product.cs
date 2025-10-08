using TalyerStudio.Inventory.Domain.Enums;

namespace TalyerStudio.Inventory.Domain.Entities;

public class Product : BaseEntity
{
    public Guid TenantId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // Category
    public Guid CategoryId { get; set; }
    public virtual ProductCategory Category { get; set; } = null!;
    
    // Product Details
    public ProductType ProductType { get; set; }
    public Applicability Applicability { get; set; } = Applicability.BOTH;
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? PartNumber { get; set; }
    
    // Pricing
    public decimal UnitPrice { get; set; }
    public decimal? CostPrice { get; set; }
    public string Currency { get; set; } = "PHP";
    
    // Stock Management
    public string Unit { get; set; } = "pcs"; // pcs, liter, kg, etc.
    public int ReorderLevel { get; set; } = 5;
    public int? MaxStockLevel { get; set; }
    
    // Supplier Info
    public string? SupplierName { get; set; }
    public string? SupplierSku { get; set; }
    
    // Additional Info
    public string? Location { get; set; } // Storage location
    public string? Barcode { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Images
    public string[] Images { get; set; } = Array.Empty<string>();
    
    // Navigation
    public virtual ICollection<StockLevel> StockLevels { get; set; } = new List<StockLevel>();
}