using Grpc.Core;
using TalyerStudio.Invoice.Application.Interfaces;
using TalyerStudio.Shared.Protos.Invoice;

namespace TalyerStudio.Invoice.API.Services;

public class InvoiceGrpcService : Shared.Protos.Invoice.InvoiceService.InvoiceServiceBase
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ILogger<InvoiceGrpcService> _logger;

    public InvoiceGrpcService(IInvoiceRepository invoiceRepository, ILogger<InvoiceGrpcService> logger)
    {
        _invoiceRepository = invoiceRepository;
        _logger = logger;
    }

    public override async Task<InvoiceResponse> GetInvoice(GetInvoiceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetInvoice called for {InvoiceId}", request.InvoiceId);

        if (!Guid.TryParse(request.InvoiceId, out var invoiceId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid invoice ID format"));
        }

        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);

        if (invoice == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Invoice not found"));
        }

        return new InvoiceResponse
        {
            Id = invoice.Id.ToString(),
            InvoiceNumber = invoice.InvoiceNumber,
            TenantId = invoice.TenantId.ToString(),
            CustomerId = invoice.CustomerId.ToString(),
            JobOrderId = invoice.JobOrderId?.ToString() ?? "",
            Status = invoice.Status,
            Subtotal = (double)invoice.Subtotal,
            TaxAmount = (double)invoice.TaxAmount,
            DiscountAmount = (double)invoice.DiscountAmount,
            TotalAmount = (double)invoice.TotalAmount,
            DueDate = invoice.DueDate?.ToString("yyyy-MM-dd") ?? ""
        };
    }

    public override async Task<InvoicesResponse> GetInvoices(GetInvoicesRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetInvoices called for tenant {TenantId}", request.TenantId);

        if (!Guid.TryParse(request.TenantId, out var tenantId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid tenant ID format"));
        }

        // Calculate page number from skip and take
        int pageSize = request.Take > 0 ? request.Take : 50;
        int pageNumber = (request.Skip / pageSize) + 1;

        var invoices = await _invoiceRepository.GetAllAsync(tenantId, pageNumber, pageSize);
        var invoiceList = invoices.ToList();

        var response = new InvoicesResponse
        {
            TotalCount = invoiceList.Count
        };

        foreach (var invoice in invoiceList)
        {
            response.Invoices.Add(new InvoiceResponse
            {
                Id = invoice.Id.ToString(),
                InvoiceNumber = invoice.InvoiceNumber,
                TenantId = invoice.TenantId.ToString(),
                CustomerId = invoice.CustomerId.ToString(),
                JobOrderId = invoice.JobOrderId?.ToString() ?? "",
                Status = invoice.Status,
                Subtotal = (double)invoice.Subtotal,
                TaxAmount = (double)invoice.TaxAmount,
                DiscountAmount = (double)invoice.DiscountAmount,
                TotalAmount = (double)invoice.TotalAmount,
                DueDate = invoice.DueDate?.ToString("yyyy-MM-dd") ?? ""
            });
        }

        return response;
    }

    public override async Task<InvoicesResponse> GetInvoicesByCustomer(GetInvoicesByCustomerRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetInvoicesByCustomer called for customer {CustomerId}", request.CustomerId);

        if (!Guid.TryParse(request.CustomerId, out var customerId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid customer ID format"));
        }

        var invoices = await _invoiceRepository.GetByCustomerIdAsync(customerId);
        var invoiceList = invoices.ToList();

        var response = new InvoicesResponse
        {
            TotalCount = invoiceList.Count
        };

        foreach (var invoice in invoiceList)
        {
            response.Invoices.Add(new InvoiceResponse
            {
                Id = invoice.Id.ToString(),
                InvoiceNumber = invoice.InvoiceNumber,
                TenantId = invoice.TenantId.ToString(),
                CustomerId = invoice.CustomerId.ToString(),
                JobOrderId = invoice.JobOrderId?.ToString() ?? "",
                Status = invoice.Status,
                Subtotal = (double)invoice.Subtotal,
                TaxAmount = (double)invoice.TaxAmount,
                DiscountAmount = (double)invoice.DiscountAmount,
                TotalAmount = (double)invoice.TotalAmount,
                DueDate = invoice.DueDate?.ToString("yyyy-MM-dd") ?? ""
            });
        }

        return response;
    }
}