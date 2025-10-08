namespace TalyerStudio.Inventory.Application.DTOs;

public class CreateProductCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public Guid? ParentCategoryId { get; set; }
}