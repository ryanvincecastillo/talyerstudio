using Microsoft.EntityFrameworkCore;
using TalyerStudio.JobOrder.Domain.Entities;
using TalyerStudio.JobOrder.Domain.Enums;

namespace TalyerStudio.JobOrder.Infrastructure.Data;

public class JobOrderDbContext : DbContext
{
    public JobOrderDbContext(DbContextOptions<JobOrderDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.JobOrder> JobOrders { get; set; }
    public DbSet<JobOrderItem> JobOrderItems { get; set; }
    public DbSet<JobOrderPart> JobOrderParts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // JobOrder configuration
        modelBuilder.Entity<Domain.Entities.JobOrder>(entity =>
        {
            entity.ToTable("job_orders");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.JobOrderNumber)
                .HasColumnName("job_order_number")
                .HasMaxLength(50)
                .IsRequired();
            
            entity.Property(e => e.TenantId)
                .HasColumnName("tenant_id")
                .IsRequired();
            
            entity.Property(e => e.BranchId)
                .HasColumnName("branch_id");
            
            entity.Property(e => e.CustomerId)
                .HasColumnName("customer_id")
                .IsRequired();
            
            entity.Property(e => e.VehicleId)
                .HasColumnName("vehicle_id")
                .IsRequired();
            
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
            
            entity.Property(e => e.Priority)
                .HasColumnName("priority")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
            
            entity.Property(e => e.OdometerReading)
                .HasColumnName("odometer_reading");
            
            entity.Property(e => e.CustomerComplaints)
                .HasColumnName("customer_complaints")
                .HasColumnType("text");
            
            entity.Property(e => e.InspectionNotes)
                .HasColumnName("inspection_notes")
                .HasColumnType("text");
            
            entity.Property(e => e.BeforePhotos)
                .HasColumnName("before_photos")
                .HasColumnType("text[]");
            
            entity.Property(e => e.AfterPhotos)
                .HasColumnName("after_photos")
                .HasColumnType("text[]");
            
            entity.Property(e => e.AssignedMechanicIds)
                .HasColumnName("assigned_mechanic_ids")
                .HasColumnType("uuid[]");
            
            entity.Property(e => e.StartTime)
                .HasColumnName("start_time");
            
            entity.Property(e => e.EndTime)
                .HasColumnName("end_time");
            
            entity.Property(e => e.EstimatedCompletionTime)
                .HasColumnName("estimated_completion_time");
            
            entity.Property(e => e.TotalAmount)
                .HasColumnName("total_amount")
                .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.DiscountAmount)
                .HasColumnName("discount_amount")
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);
            
            entity.Property(e => e.TaxAmount)
                .HasColumnName("tax_amount")
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);
            
            entity.Property(e => e.GrandTotal)
                .HasColumnName("grand_total")
                .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at");

            // Indexes
            entity.HasIndex(e => e.JobOrderNumber)
                .IsUnique()
                .HasDatabaseName("idx_job_orders_number");
            
            entity.HasIndex(e => e.TenantId)
                .HasDatabaseName("idx_job_orders_tenant_id");
            
            entity.HasIndex(e => e.CustomerId)
                .HasDatabaseName("idx_job_orders_customer_id");
            
            entity.HasIndex(e => e.VehicleId)
                .HasDatabaseName("idx_job_orders_vehicle_id");
            
            entity.HasIndex(e => e.Status)
                .HasDatabaseName("idx_job_orders_status");
            
            entity.HasIndex(e => new { e.TenantId, e.Status })
                .HasDatabaseName("idx_job_orders_tenant_status");

            // Relationships
            entity.HasMany(e => e.Items)
                .WithOne(i => i.JobOrder)
                .HasForeignKey(i => i.JobOrderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasMany(e => e.Parts)
                .WithOne(p => p.JobOrder)
                .HasForeignKey(p => p.JobOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // JobOrderItem configuration
        modelBuilder.Entity<JobOrderItem>(entity =>
        {
            entity.ToTable("job_order_items");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.JobOrderId)
                .HasColumnName("job_order_id")
                .IsRequired();
            
            entity.Property(e => e.ServiceId)
                .HasColumnName("service_id")
                .IsRequired();
            
            entity.Property(e => e.ServiceName)
                .HasColumnName("service_name")
                .HasMaxLength(200)
                .IsRequired();
            
            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);
            
            entity.Property(e => e.Quantity)
                .HasColumnName("quantity")
                .HasDefaultValue(1);
            
            entity.Property(e => e.UnitPrice)
                .HasColumnName("unit_price")
                .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.Subtotal)
                .HasColumnName("subtotal")
                .HasColumnType("decimal(18,2)");
            
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
            entity.HasIndex(e => e.JobOrderId)
                .HasDatabaseName("idx_job_order_items_job_order_id");
            
            entity.HasIndex(e => e.ServiceId)
                .HasDatabaseName("idx_job_order_items_service_id");
        });

        // JobOrderPart configuration
        modelBuilder.Entity<JobOrderPart>(entity =>
        {
            entity.ToTable("job_order_parts");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.JobOrderId)
                .HasColumnName("job_order_id")
                .IsRequired();
            
            entity.Property(e => e.ProductId)
                .HasColumnName("product_id")
                .IsRequired();
            
            entity.Property(e => e.ProductName)
                .HasColumnName("product_name")
                .HasMaxLength(200)
                .IsRequired();
            
            entity.Property(e => e.ProductSku)
                .HasColumnName("product_sku")
                .HasMaxLength(50);
            
            entity.Property(e => e.Quantity)
                .HasColumnName("quantity")
                .IsRequired();
            
            entity.Property(e => e.UnitPrice)
                .HasColumnName("unit_price")
                .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.Subtotal)
                .HasColumnName("subtotal")
                .HasColumnType("decimal(18,2)");
            
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
            entity.HasIndex(e => e.JobOrderId)
                .HasDatabaseName("idx_job_order_parts_job_order_id");
            
            entity.HasIndex(e => e.ProductId)
                .HasDatabaseName("idx_job_order_parts_product_id");
        });
    }
}