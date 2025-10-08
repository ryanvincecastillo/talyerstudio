using Microsoft.EntityFrameworkCore;
using TalyerStudio.Customer.Domain.Entities;

namespace TalyerStudio.Customer.Infrastructure.Data;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Domain.Entities.Customer> Customers { get; set; }
    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<Service> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Entities.Customer>(entity =>
        {
            entity.ToTable("customers");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.TenantId)
                .HasColumnName("tenant_id")
                .IsRequired();
            
            entity.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(255);
            
            entity.Property(e => e.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(20)
                .IsRequired();
            
            entity.Property(e => e.Street)
                .HasColumnName("street")
                .HasMaxLength(255);
            
            entity.Property(e => e.Barangay)
                .HasColumnName("barangay")
                .HasMaxLength(100);
            
            entity.Property(e => e.Municipality)
                .HasColumnName("municipality")
                .HasMaxLength(100);
            
            entity.Property(e => e.Province)
                .HasColumnName("province")
                .HasMaxLength(100);
            
            entity.Property(e => e.ZipCode)
                .HasColumnName("zip_code")
                .HasMaxLength(10);
            
            entity.Property(e => e.Birthday)
                .HasColumnName("birthday");
            
            entity.Property(e => e.LoyaltyPoints)
                .HasColumnName("loyalty_points")
                .HasDefaultValue(0);
            
            entity.Property(e => e.CustomerType)
                .HasColumnName("customer_type")
                .HasMaxLength(20)
                .HasDefaultValue("INDIVIDUAL");
            
            entity.Property(e => e.Tags)
                .HasColumnName("tags")
                .HasColumnType("text[]");
            
            entity.Property(e => e.Notes)
                .HasColumnName("notes")
                .HasColumnType("text");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at");

            // Indexes
            entity.HasIndex(e => e.TenantId)
                .HasDatabaseName("idx_customers_tenant_id");
            
            entity.HasIndex(e => e.PhoneNumber)
                .HasDatabaseName("idx_customers_phone");
            
            entity.HasIndex(e => e.Email)
                .HasDatabaseName("idx_customers_email");
        });
        
         // ServiceCategory configuration
        modelBuilder.Entity<ServiceCategory>(entity =>
        {
            entity.ToTable("service_categories");
            
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
            
            entity.Property(e => e.DisplayOrder)
                .HasColumnName("display_order")
                .HasDefaultValue(0);
            
            entity.Property(e => e.Icon)
                .HasColumnName("icon")
                .HasMaxLength(50);
            
            entity.Property(e => e.Color)
                .HasColumnName("color")
                .HasMaxLength(20);
            
            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at");

            // Indexes
            entity.HasIndex(e => e.TenantId)
                .HasDatabaseName("idx_service_categories_tenant_id");
        });

        // Service configuration
        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable("services");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.TenantId)
                .HasColumnName("tenant_id")
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
            
            entity.Property(e => e.BasePrice)
                .HasColumnName("base_price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            entity.Property(e => e.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3)
                .HasDefaultValue("PHP");
            
            entity.Property(e => e.Applicability)
                .HasColumnName("applicability")
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(ServiceApplicability.BOTH);
            
            entity.Property(e => e.EstimatedDurationMinutes)
                .HasColumnName("estimated_duration_minutes");
            
            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            
            entity.Property(e => e.DisplayOrder)
                .HasColumnName("display_order")
                .HasDefaultValue(0);
            
            entity.Property(e => e.Icon)
                .HasColumnName("icon")
                .HasMaxLength(50);
            
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
                .WithMany(c => c.Services)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.TenantId)
                .HasDatabaseName("idx_services_tenant_id");
            
            entity.HasIndex(e => e.CategoryId)
                .HasDatabaseName("idx_services_category_id");
            
            entity.HasIndex(e => e.IsActive)
                .HasDatabaseName("idx_services_is_active");
        });
    }
}
