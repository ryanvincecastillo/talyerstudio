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
    }
}
