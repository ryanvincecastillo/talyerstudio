using Microsoft.EntityFrameworkCore;
using TalyerStudio.Invoice.Application.DTOs;
using TalyerStudio.Invoice.Application.Interfaces;
using TalyerStudio.Invoice.Domain.Enums;
using TalyerStudio.Invoice.Infrastructure.Data;

namespace TalyerStudio.Invoice.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly InvoiceDbContext _context;

    public InvoiceRepository(InvoiceDbContext context)
    {
        _context = context;
    }

    public async Task<InvoiceDto?> GetByIdAsync(Guid id)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Items)
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);

        return invoice == null ? null : MapToDto(invoice);
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllAsync(Guid tenantId, int pageNumber = 1, int pageSize = 50)
    {
        var invoices = await _context.Invoices
            .Include(i => i.Items)
            .Include(i => i.Payments)
            .Where(i => i.TenantId == tenantId && !i.IsDeleted)
            .OrderByDescending(i => i.InvoiceDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return invoices.Select(MapToDto);
    }

    public async Task<IEnumerable<InvoiceDto>> GetByCustomerIdAsync(Guid customerId)
    {
        var invoices = await _context.Invoices
            .Include(i => i.Items)
            .Include(i => i.Payments)
            .Where(i => i.CustomerId == customerId && !i.IsDeleted)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync();

        return invoices.Select(MapToDto);
    }

    public async Task<IEnumerable<InvoiceDto>> GetByJobOrderIdAsync(Guid jobOrderId)
    {
        var invoices = await _context.Invoices
            .Include(i => i.Items)
            .Include(i => i.Payments)
            .Where(i => i.JobOrderId == jobOrderId && !i.IsDeleted)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync();

        return invoices.Select(MapToDto);
    }

    public async Task<InvoiceDto?> GetByInvoiceNumberAsync(string invoiceNumber)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Items)
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber && !i.IsDeleted);

        return invoice == null ? null : MapToDto(invoice);
    }

    public async Task<InvoiceDto> CreateAsync(CreateInvoiceDto createDto)
    {
        var invoice = new Domain.Entities.Invoice
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = await GenerateInvoiceNumberAsync(),
            TenantId = createDto.TenantId,
            BranchId = createDto.BranchId,
            CustomerId = createDto.CustomerId,
            JobOrderId = createDto.JobOrderId,
            InvoiceDate = DateTime.UtcNow,
            DueDate = createDto.DueDate,
            Status = InvoiceStatus.PENDING,
            Notes = createDto.Notes,
            Terms = createDto.Terms,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        // Add items and calculate totals
        decimal subtotal = 0;
        decimal totalTax = 0;
        decimal totalDiscount = createDto.DiscountAmount;

        foreach (var itemDto in createDto.Items)
        {
            var itemSubtotal = itemDto.Quantity * itemDto.UnitPrice;
            var itemDiscount = itemDto.DiscountAmount;
            var itemTaxableAmount = itemSubtotal - itemDiscount;
            var itemTax = itemTaxableAmount * createDto.TaxRate;
            var itemTotal = itemTaxableAmount + itemTax;

            var invoiceItem = new Domain.Entities.InvoiceItem
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoice.Id,
                ItemType = itemDto.ItemType,
                ItemId = itemDto.ItemId,
                Description = itemDto.Description,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                DiscountAmount = itemDiscount,
                TaxAmount = itemTax,
                TotalAmount = itemTotal
            };

            invoice.Items.Add(invoiceItem);
            subtotal += itemSubtotal;
            totalTax += itemTax;
        }

        invoice.Subtotal = subtotal;
        invoice.DiscountAmount = totalDiscount;
        invoice.TaxAmount = totalTax;
        invoice.TotalAmount = subtotal - totalDiscount + totalTax;
        invoice.AmountPaid = 0;
        invoice.Balance = invoice.TotalAmount;

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        return MapToDto(invoice);
    }

    public async Task<PaymentDto> AddPaymentAsync(CreatePaymentDto paymentDto)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.Id == paymentDto.InvoiceId && !i.IsDeleted);

        if (invoice == null)
            throw new Exception("Invoice not found");

        if (paymentDto.Amount <= 0)
            throw new Exception("Payment amount must be greater than zero");

        if (paymentDto.Amount > invoice.Balance)
            throw new Exception("Payment amount exceeds invoice balance");

        var payment = new Domain.Entities.Payment
        {
            Id = Guid.NewGuid(),
            InvoiceId = paymentDto.InvoiceId,
            PaymentNumber = await GeneratePaymentNumberAsync(),
            Amount = paymentDto.Amount,
            PaymentMethod = Enum.Parse<PaymentMethod>(paymentDto.PaymentMethod),
            Status = PaymentStatus.COMPLETED,
            PaymentDate = paymentDto.PaymentDate,
            ReferenceNumber = paymentDto.ReferenceNumber,
            Notes = paymentDto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);

        // Update invoice amounts
        invoice.AmountPaid += paymentDto.Amount;
        invoice.Balance -= paymentDto.Amount;

        // Update invoice status
        if (invoice.Balance <= 0)
        {
            invoice.Status = InvoiceStatus.PAID;
            invoice.PaidDate = DateTime.UtcNow;
        }
        else if (invoice.AmountPaid > 0)
        {
            invoice.Status = InvoiceStatus.PARTIALLY_PAID;
        }

        invoice.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapPaymentToDto(payment);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null) return false;

        invoice.IsDeleted = true;
        invoice.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<string> GenerateInvoiceNumberAsync()
    {
        var today = DateTime.UtcNow;
        var prefix = $"INV-{today:yyyyMMdd}";

        var lastInvoice = await _context.Invoices
            .Where(i => i.InvoiceNumber.StartsWith(prefix))
            .OrderByDescending(i => i.InvoiceNumber)
            .FirstOrDefaultAsync();

        int sequence = 1;
        if (lastInvoice != null)
        {
            var lastSequence = lastInvoice.InvoiceNumber.Split('-').Last();
            if (int.TryParse(lastSequence, out int lastNumber))
            {
                sequence = lastNumber + 1;
            }
        }

        return $"{prefix}-{sequence:D4}";
    }

    private async Task<string> GeneratePaymentNumberAsync()
    {
        var today = DateTime.UtcNow;
        var prefix = $"PAY-{today:yyyyMMdd}";

        var lastPayment = await _context.Payments
            .Where(p => p.PaymentNumber.StartsWith(prefix))
            .OrderByDescending(p => p.PaymentNumber)
            .FirstOrDefaultAsync();

        int sequence = 1;
        if (lastPayment != null)
        {
            var lastSequence = lastPayment.PaymentNumber.Split('-').Last();
            if (int.TryParse(lastSequence, out int lastNumber))
            {
                sequence = lastNumber + 1;
            }
        }

        return $"{prefix}-{sequence:D4}";
    }

    private static InvoiceDto MapToDto(Domain.Entities.Invoice invoice)
    {
        return new InvoiceDto
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            TenantId = invoice.TenantId,
            BranchId = invoice.BranchId,
            CustomerId = invoice.CustomerId,
            JobOrderId = invoice.JobOrderId,
            Subtotal = invoice.Subtotal,
            TaxAmount = invoice.TaxAmount,
            DiscountAmount = invoice.DiscountAmount,
            TotalAmount = invoice.TotalAmount,
            AmountPaid = invoice.AmountPaid,
            Balance = invoice.Balance,
            Status = invoice.Status.ToString(),
            InvoiceDate = invoice.InvoiceDate,
            DueDate = invoice.DueDate,
            PaidDate = invoice.PaidDate,
            Notes = invoice.Notes,
            Terms = invoice.Terms,
            CreatedAt = invoice.CreatedAt,
            UpdatedAt = invoice.UpdatedAt,
            Items = invoice.Items.Select(MapItemToDto).ToList(),
            Payments = invoice.Payments.Select(MapPaymentToDto).ToList()
        };
    }

    private static InvoiceItemDto MapItemToDto(Domain.Entities.InvoiceItem item)
    {
        return new InvoiceItemDto
        {
            Id = item.Id,
            InvoiceId = item.InvoiceId,
            ItemType = item.ItemType,
            ItemId = item.ItemId,
            Description = item.Description,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            DiscountAmount = item.DiscountAmount,
            TaxAmount = item.TaxAmount,
            TotalAmount = item.TotalAmount
        };
    }

    private static PaymentDto MapPaymentToDto(Domain.Entities.Payment payment)
    {
        return new PaymentDto
        {
            Id = payment.Id,
            InvoiceId = payment.InvoiceId,
            PaymentNumber = payment.PaymentNumber,
            Amount = payment.Amount,
            PaymentMethod = payment.PaymentMethod.ToString(),
            Status = payment.Status.ToString(),
            PaymentDate = payment.PaymentDate,
            ReferenceNumber = payment.ReferenceNumber,
            Notes = payment.Notes,
            CreatedAt = payment.CreatedAt
        };
    }
}