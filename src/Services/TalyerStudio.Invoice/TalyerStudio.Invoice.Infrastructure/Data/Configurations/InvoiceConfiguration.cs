using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalyerStudio.Invoice.Domain.Enums;

namespace TalyerStudio.Invoice.Infrastructure.Data.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Domain.Entities.Invoice>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Invoice> builder)
    {
        builder.ToTable("invoices");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.InvoiceNumber)
            .HasColumnName("invoice_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(i => i.InvoiceNumber)
            .IsUnique();

        builder.Property(i => i.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(i => i.BranchId)
            .HasColumnName("branch_id");

        builder.Property(i => i.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();

        builder.Property(i => i.JobOrderId)
            .HasColumnName("job_order_id");

        builder.Property(i => i.Subtotal)
            .HasColumnName("subtotal")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.TaxAmount)
            .HasColumnName("tax_amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.DiscountAmount)
            .HasColumnName("discount_amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.TotalAmount)
            .HasColumnName("total_amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.AmountPaid)
            .HasColumnName("amount_paid")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.Balance)
            .HasColumnName("balance")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(i => i.InvoiceDate)
            .HasColumnName("invoice_date")
            .IsRequired();

        builder.Property(i => i.DueDate)
            .HasColumnName("due_date");

        builder.Property(i => i.PaidDate)
            .HasColumnName("paid_date");

        builder.Property(i => i.Notes)
            .HasColumnName("notes")
            .HasMaxLength(1000);

        builder.Property(i => i.Terms)
            .HasColumnName("terms")
            .HasMaxLength(500);

        builder.Property(i => i.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(i => i.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(i => i.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100);

        builder.Property(i => i.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(100);

        builder.Property(i => i.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        // Relationships
        builder.HasMany(i => i.Items)
            .WithOne(ii => ii.Invoice)
            .HasForeignKey(ii => ii.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(i => i.Payments)
            .WithOne(p => p.Invoice)
            .HasForeignKey(p => p.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(i => i.TenantId);
        builder.HasIndex(i => i.CustomerId);
        builder.HasIndex(i => i.JobOrderId);
        builder.HasIndex(i => i.Status);
        builder.HasIndex(i => i.InvoiceDate);
    }
}