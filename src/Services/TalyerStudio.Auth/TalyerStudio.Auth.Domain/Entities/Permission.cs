namespace TalyerStudio.Auth.Domain.Entities;

public class Permission
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g., "customers.create"
    public string Description { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty; // e.g., "customers", "vehicles"
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}