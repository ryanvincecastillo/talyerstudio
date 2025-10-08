using Microsoft.AspNetCore.Mvc;
using TalyerStudio.Invoice.Application.DTOs;
using TalyerStudio.Invoice.Application.Interfaces;

namespace TalyerStudio.Invoice.API.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ILogger<InvoicesController> _logger;

    public InvoicesController(
        IInvoiceRepository invoiceRepository,
        ILogger<InvoicesController> logger)
    {
        _invoiceRepository = invoiceRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all invoices for a tenant
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAll(
        [FromQuery] Guid tenantId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var invoices = await _invoiceRepository.GetAllAsync(tenantId, pageNumber, pageSize);
            return Ok(invoices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving invoices");
            return StatusCode(500, "An error occurred while retrieving invoices");
        }
    }

    /// <summary>
    /// Get invoice by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceDto>> GetById(Guid id)
    {
        try
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                return NotFound($"Invoice with ID {id} not found");

            return Ok(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving invoice {InvoiceId}", id);
            return StatusCode(500, "An error occurred while retrieving the invoice");
        }
    }

    /// <summary>
    /// Get invoice by invoice number
    /// </summary>
    [HttpGet("number/{invoiceNumber}")]
    public async Task<ActionResult<InvoiceDto>> GetByInvoiceNumber(string invoiceNumber)
    {
        try
        {
            var invoice = await _invoiceRepository.GetByInvoiceNumberAsync(invoiceNumber);
            if (invoice == null)
                return NotFound($"Invoice {invoiceNumber} not found");

            return Ok(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving invoice {InvoiceNumber}", invoiceNumber);
            return StatusCode(500, "An error occurred while retrieving the invoice");
        }
    }

    /// <summary>
    /// Get invoices by customer ID
    /// </summary>
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetByCustomerId(Guid customerId)
    {
        try
        {
            var invoices = await _invoiceRepository.GetByCustomerIdAsync(customerId);
            return Ok(invoices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving invoices for customer {CustomerId}", customerId);
            return StatusCode(500, "An error occurred while retrieving invoices");
        }
    }

    /// <summary>
    /// Get invoices by job order ID
    /// </summary>
    [HttpGet("job-order/{jobOrderId}")]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetByJobOrderId(Guid jobOrderId)
    {
        try
        {
            var invoices = await _invoiceRepository.GetByJobOrderIdAsync(jobOrderId);
            return Ok(invoices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving invoices for job order {JobOrderId}", jobOrderId);
            return StatusCode(500, "An error occurred while retrieving invoices");
        }
    }

    /// <summary>
    /// Create a new invoice
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<InvoiceDto>> Create([FromBody] CreateInvoiceDto createDto)
    {
        try
        {
            if (createDto.Items == null || !createDto.Items.Any())
                return BadRequest("Invoice must have at least one item");

            var invoice = await _invoiceRepository.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating invoice");
            return StatusCode(500, "An error occurred while creating the invoice");
        }
    }

    /// <summary>
    /// Delete an invoice (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _invoiceRepository.DeleteAsync(id);
            if (!result)
                return NotFound($"Invoice with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting invoice {InvoiceId}", id);
            return StatusCode(500, "An error occurred while deleting the invoice");
        }
    }
}