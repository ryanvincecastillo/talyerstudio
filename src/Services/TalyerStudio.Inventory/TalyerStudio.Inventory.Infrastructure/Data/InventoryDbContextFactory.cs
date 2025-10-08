using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TalyerStudio.Inventory.Infrastructure.Data;

public class InventoryDbContextFactory : IDesignTimeDbContextFactory<InventoryDbContext>
{
    public InventoryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
        
        // Temporary connection string for migrations
        optionsBuilder.UseNpgsql("Host=localhost;Database=talyerstudio_inventory;Username=postgres;Password=postgres");
        
        return new InventoryDbContext(optionsBuilder.Options);
    }
}