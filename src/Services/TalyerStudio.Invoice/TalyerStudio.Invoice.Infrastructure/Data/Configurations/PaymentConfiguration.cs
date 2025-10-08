using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalyerStudio.Invoice.Domain.Entities;

namespace TalyerStudio.Invoice.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(p => p.InvoiceId)
            .HasColumnName("invoice_id")
            .IsRequired();

        builder.Property(p => p.PaymentNumber)
            .HasColumnName("payment_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(p => p.PaymentNumber)
            .IsUnique();

        builder.Property(p => p.Amount)
            .HasColumnName("amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.PaymentMethod)
            .HasColumnName("payment_method")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.PaymentDate)
            .HasColumnName("payment_date")
            .IsRequired();

        builder.Property(p => p.ReferenceNumber)
            .HasColumnName("reference_number")
            .HasMaxLength(100);

        builder.Property(p => p.Notes)
            .HasColumnName("notes")
            .HasMaxLength(500);

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(p => p.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(p => p.InvoiceId);
        builder.HasIndex(p => p.PaymentDate);
        builder.HasIndex(p => p.Status);
    }
}