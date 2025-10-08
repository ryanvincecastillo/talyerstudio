using Microsoft.AspNetCore.Mvc;
using TalyerStudio.Invoice.Application.DTOs;
using TalyerStudio.Invoice.Application.Interfaces;

namespace TalyerStudio.Invoice.API.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(
        IInvoiceRepository invoiceRepository,
        ILogger<PaymentsController> logger)
    {
        _invoiceRepository = invoiceRepository;
        _logger = logger;
    }

    /// <summary>
    /// Record a payment for an invoice
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PaymentDto>> RecordPayment([FromBody] CreatePaymentDto paymentDto)
    {
        try
        {
            if (paymentDto.Amount <= 0)
                return BadRequest("Payment amount must be greater than zero");

            var payment = await _invoiceRepository.AddPaymentAsync(paymentDto);
            return CreatedAtAction(nameof(GetInvoiceWithPayments), 
                new { invoiceId = payment.InvoiceId }, payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording payment for invoice {InvoiceId}", paymentDto.InvoiceId);
            
            if (ex.Message.Contains("not found") || ex.Message.Contains("exceeds"))
                return BadRequest(ex.Message);
            
            return StatusCode(500, "An error occurred while recording the payment");
        }
    }

    /// <summary>
    /// Get invoice with all payments
    /// </summary>
    [HttpGet("invoice/{invoiceId}")]
    public async Task<ActionResult<InvoiceDto>> GetInvoiceWithPayments(Guid invoiceId)
    {
        try
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice == null)
                return NotFound($"Invoice with ID {invoiceId} not found");

            return Ok(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving invoice {InvoiceId} with payments", invoiceId);
            return StatusCode(500, "An error occurred while retrieving the invoice");
        }
    }
}