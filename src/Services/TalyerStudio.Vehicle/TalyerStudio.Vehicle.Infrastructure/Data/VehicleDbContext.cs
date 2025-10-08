using Microsoft.EntityFrameworkCore;
using TalyerStudio.Vehicle.Domain.Entities;

namespace TalyerStudio.Vehicle.Infrastructure.Data;

public class VehicleDbContext : DbContext
{
    public VehicleDbContext(DbContextOptions<VehicleDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Domain.Entities.Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Entities.Vehicle>(entity =>
        {
            entity.ToTable("vehicles");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.TenantId)
                .HasColumnName("tenant_id")
                .IsRequired();
            
            entity.Property(e => e.CustomerId)
                .HasColumnName("customer_id")
                .IsRequired();
            
            entity.Property(e => e.Make)
                .HasColumnName("make")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.Model)
                .HasColumnName("model")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.Year)
                .HasColumnName("year")
                .IsRequired();
            
            entity.Property(e => e.Color)
                .HasColumnName("color")
                .HasMaxLength(50);
            
            entity.Property(e => e.PlateNumber)
                .HasColumnName("plate_number")
                .HasMaxLength(20)
                .IsRequired();
            
            entity.Property(e => e.EngineNumber)
                .HasColumnName("engine_number")
                .HasMaxLength(50);
            
            entity.Property(e => e.ChassisNumber)
                .HasColumnName("chassis_number")
                .HasMaxLength(50);
            
            entity.Property(e => e.VehicleType)
                .HasColumnName("vehicle_type")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
            
            entity.Property(e => e.VehicleCategory)
                .HasColumnName("vehicle_category")
                .HasMaxLength(50);
            
            entity.Property(e => e.Displacement)
                .HasColumnName("displacement");
            
            entity.Property(e => e.FuelType)
                .HasColumnName("fuel_type")
                .HasMaxLength(20);
            
            entity.Property(e => e.Transmission)
                .HasColumnName("transmission")
                .HasMaxLength(20);
            
            entity.Property(e => e.CurrentOdometer)
                .HasColumnName("current_odometer")
                .HasDefaultValue(0);
            
            entity.Property(e => e.OrCrExpiryDate)
                .HasColumnName("or_cr_expiry_date");
            
            entity.Property(e => e.TireSizeFront)
                .HasColumnName("tire_size_front")
                .HasMaxLength(20);
            
            entity.Property(e => e.TireSizeRear)
                .HasColumnName("tire_size_rear")
                .HasMaxLength(20);
            
            entity.Property(e => e.Images)
                .HasColumnName("images")
                .HasColumnType("text[]");
            
            entity.Property(e => e.QrCode)
                .HasColumnName("qr_code")
                .HasMaxLength(100);
            
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
                .HasDatabaseName("idx_vehicles_tenant_id");
            
            entity.HasIndex(e => e.CustomerId)
                .HasDatabaseName("idx_vehicles_customer_id");
            
            entity.HasIndex(e => e.PlateNumber)
                .HasDatabaseName("idx_vehicles_plate_number");
        });
    }
}