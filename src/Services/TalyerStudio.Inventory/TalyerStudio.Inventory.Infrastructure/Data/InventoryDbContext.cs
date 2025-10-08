using Microsoft.EntityFrameworkCore;
using TalyerStudio.Inventory.Domain.Entities;
using TalyerStudio.Inventory.Domain.Enums;

namespace TalyerStudio.Inventory.Infrastructure.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<StockLevel> StockLevels { get; set; }
    public DbSet<StockMovement> StockMovements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ProductCategory configuration
        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.ToTable("product_categories");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.TenantId)
                .HasColumnName("tenant_id")
                .IsRequired();
            
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(500);
            
            entity.Property(e => e.Icon)
                .HasColumnName("icon")
                .HasMaxLength(50);
            
            entity.Property(e => e.Color)
                .HasColumnName("color")
                .HasMaxLength(20);
            
            entity.Property(e => e.DisplayOrder)
                .HasColumnName("display_order")
                .HasDefaultValue(0);
            
            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            
            entity.Property(e => e.ParentCategoryId)
                .HasColumnName("parent_category_id");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at");

            // Self-referencing relationship
            entity.HasOne(e => e.ParentCategory)
                .WithMany(e => e.SubCategories)
                .HasForeignKey(e => e.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.TenantId)
                .HasDatabaseName("idx_product_categories_tenant_id");
            
            entity.HasIndex(e => e.ParentCategoryId)
                .HasDatabaseName("idx_product_categories_parent_id");
        });

        // Product configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.TenantId)
                .HasColumnName("tenant_id")
                .IsRequired();
            
            entity.Property(e => e.Sku)
                .HasColumnName("sku")
                .HasMaxLength(50)
                .IsRequired();
            
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();
            
            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);
            
            entity.Property(e => e.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();
            
            entity.Property(e => e.ProductType)
                .HasColumnName("product_type")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
            
            entity.Property(e => e.Applicability)
                .HasColumnName("applicability")
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(Applicability.BOTH);
            
            entity.Property(e => e.Brand)
                .HasColumnName("brand")
                .HasMaxLength(100);
            
            entity.Property(e => e.Model)
                .HasColumnName("model")
                .HasMaxLength(100);
            
            entity.Property(e => e.PartNumber)
                .HasColumnName("part_number")
                .HasMaxLength(100);
            
            entity.Property(e => e.UnitPrice)
                .HasColumnName("unit_price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            entity.Property(e => e.CostPrice)
                .HasColumnName("cost_price")
                .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3)
                .HasDefaultValue("PHP");
            
            entity.Property(e => e.Unit)
                .HasColumnName("unit")
                .HasMaxLength(20)
                .HasDefaultValue("pcs");
            
            entity.Property(e => e.ReorderLevel)
                .HasColumnName("reorder_level")
                .HasDefaultValue(5);
            
            entity.Property(e => e.MaxStockLevel)
                .HasColumnName("max_stock_level");
            
            entity.Property(e => e.SupplierName)
                .HasColumnName("supplier_name")
                .HasMaxLength(200);
            
            entity.Property(e => e.SupplierSku)
                .HasColumnName("supplier_sku")
                .HasMaxLength(100);
            
            entity.Property(e => e.Location)
                .HasColumnName("location")
                .HasMaxLength(100);
            
            entity.Property(e => e.Barcode)
                .HasColumnName("barcode")
                .HasMaxLength(100);
            
            entity.Property(e => e.Notes)
                .HasColumnName("notes")
                .HasColumnType("text");
            
            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            
            entity.Property(e => e.Images)
                .HasColumnName("images")
                .HasColumnType("text[]");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at");

            // Relationships
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.StockLevels)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.Sku)
                .IsUnique()
                .HasDatabaseName("idx_products_sku");
            
            entity.HasIndex(e => e.TenantId)
                .HasDatabaseName("idx_products_tenant_id");
            
            entity.HasIndex(e => e.CategoryId)
                .HasDatabaseName("idx_products_category_id");
            
            entity.HasIndex(e => e.IsActive)
                .HasDatabaseName("idx_products_is_active");
            
            entity.HasIndex(e => new { e.TenantId, e.IsActive })
                .HasDatabaseName("idx_products_tenant_active");
        });

        // StockLevel configuration
        modelBuilder.Entity<StockLevel>(entity =>
        {
            entity.ToTable("stock_levels");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.ProductId)
                .HasColumnName("product_id")
                .IsRequired();
            
            entity.Property(e => e.BranchId)
                .HasColumnName("branch_id");
            
            entity.Property(e => e.CurrentQuantity)
                .HasColumnName("current_quantity")
                .HasDefaultValue(0);
            
            entity.Property(e => e.ReservedQuantity)
                .HasColumnName("reserved_quantity")
                .HasDefaultValue(0);
            
            entity.Property(e => e.LastRestockDate)
                .HasColumnName("last_restock_date");
            
            entity.Property(e => e.LastCountDate)
                .HasColumnName("last_count_date");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at");

            // Indexes
            entity.HasIndex(e => e.ProductId)
                .HasDatabaseName("idx_stock_levels_product_id");
            
            entity.HasIndex(e => new { e.ProductId, e.BranchId })
                .IsUnique()
                .HasDatabaseName("idx_stock_levels_product_branch");
        });

        // StockMovement configuration
        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.ToTable("stock_movements");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.ProductId)
                .HasColumnName("product_id")
                .IsRequired();
            
            entity.Property(e => e.BranchId)
                .HasColumnName("branch_id");
            
            entity.Property(e => e.MovementType)
                .HasColumnName("movement_type")
                .HasMaxLength(20)
                .IsRequired();
            
            entity.Property(e => e.Quantity)
                .HasColumnName("quantity")
                .IsRequired();
            
            entity.Property(e => e.PreviousQuantity)
                .HasColumnName("previous_quantity");
            
            entity.Property(e => e.NewQuantity)
                .HasColumnName("new_quantity");
            
            entity.Property(e => e.Reference)
                .HasColumnName("reference")
                .HasMaxLength(100);
            
            entity.Property(e => e.Reason)
                .HasColumnName("reason")
                .HasMaxLength(200);
            
            entity.Property(e => e.Notes)
                .HasColumnName("notes")
                .HasColumnType("text");
            
            entity.Property(e => e.PerformedBy)
                .HasColumnName("performed_by");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at");

            // Indexes
            entity.HasIndex(e => e.ProductId)
                .HasDatabaseName("idx_stock_movements_product_id");
            
            entity.HasIndex(e => e.Reference)
                .HasDatabaseName("idx_stock_movements_reference");
            
            entity.HasIndex(e => e.CreatedAt)
                .HasDatabaseName("idx_stock_movements_created_at");
        });
    }
}