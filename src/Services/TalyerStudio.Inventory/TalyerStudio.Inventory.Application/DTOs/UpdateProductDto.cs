namespace TalyerStudio.Inventory.Application.DTOs;

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public Guid CategoryId { get; set; }
    
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? PartNumber { get; set; }
    
    public decimal UnitPrice { get; set; }
    public decimal? CostPrice { get; set; }
    
    public int ReorderLevel { get; set; }
    public int? MaxStockLevel { get; set; }
    
    public string? SupplierName { get; set; }
    public string? SupplierSku { get; set; }
    
    public string? Location { get; set; }
    public string? Barcode { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
}