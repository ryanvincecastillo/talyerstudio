using TalyerStudio.Invoice.Application.DTOs;

namespace TalyerStudio.Invoice.Application.Interfaces;

public interface IInvoiceRepository
{
    Task<InvoiceDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<InvoiceDto>> GetAllAsync(Guid tenantId, int pageNumber = 1, int pageSize = 50);
    Task<IEnumerable<InvoiceDto>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<InvoiceDto>> GetByJobOrderIdAsync(Guid jobOrderId);
    Task<InvoiceDto> CreateAsync(CreateInvoiceDto createDto);
    Task<PaymentDto> AddPaymentAsync(CreatePaymentDto paymentDto);
    Task<bool> DeleteAsync(Guid id);
    Task<InvoiceDto?> GetByInvoiceNumberAsync(string invoiceNumber);
}