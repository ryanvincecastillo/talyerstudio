namespace TalyerStudio.Inventory.Application.DTOs;

public class CreateProductDto
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public Guid CategoryId { get; set; }
    
    public string ProductType { get; set; } = "PART";
    public string Applicability { get; set; } = "BOTH";
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? PartNumber { get; set; }
    
    public decimal UnitPrice { get; set; }
    public decimal? CostPrice { get; set; }
    
    public string Unit { get; set; } = "pcs";
    public int ReorderLevel { get; set; } = 5;
    public int? MaxStockLevel { get; set; }
    
    public string? SupplierName { get; set; }
    public string? SupplierSku { get; set; }
    
    public string? Location { get; set; }
    public string? Barcode { get; set; }
    public string? Notes { get; set; }
    
    public int InitialStock { get; set; } = 0;
}