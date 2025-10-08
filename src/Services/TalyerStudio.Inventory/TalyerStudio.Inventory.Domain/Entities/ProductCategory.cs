namespace TalyerStudio.Inventory.Domain.Entities;

public class ProductCategory : BaseEntity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    
    // Parent Category (for hierarchy)
    public Guid? ParentCategoryId { get; set; }
    public virtual ProductCategory? ParentCategory { get; set; }
    
    // Navigation Properties
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<ProductCategory> SubCategories { get; set; } = new List<ProductCategory>();
}