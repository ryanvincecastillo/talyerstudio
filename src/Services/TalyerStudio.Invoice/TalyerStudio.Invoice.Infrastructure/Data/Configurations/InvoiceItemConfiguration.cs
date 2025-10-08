using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalyerStudio.Invoice.Domain.Entities;

namespace TalyerStudio.Invoice.Infrastructure.Data.Configurations;

public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.ToTable("invoice_items");

        builder.HasKey(ii => ii.Id);

        builder.Property(ii => ii.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(ii => ii.InvoiceId)
            .HasColumnName("invoice_id")
            .IsRequired();

        builder.Property(ii => ii.ItemType)
            .HasColumnName("item_type")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(ii => ii.ItemId)
            .HasColumnName("item_id");

        builder.Property(ii => ii.Description)
            .HasColumnName("description")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(ii => ii.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(ii => ii.UnitPrice)
            .HasColumnName("unit_price")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(ii => ii.DiscountAmount)
            .HasColumnName("discount_amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(ii => ii.TaxAmount)
            .HasColumnName("tax_amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(ii => ii.TotalAmount)
            .HasColumnName("total_amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        // Indexes
        builder.HasIndex(ii => ii.InvoiceId);
        builder.HasIndex(ii => ii.ItemType);
    }
}