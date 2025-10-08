using Microsoft.AspNetCore.Mvc;
using TalyerStudio.Customer.Application.Interfaces;
using TalyerStudio.Shared.Contracts.Customers;

namespace TalyerStudio.Customer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    // GET: api/customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers(
        [FromQuery] string? search = null,
        [FromQuery] string? tag = null,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        try
        {
            // TODO: Get tenantId from authenticated user context
            var tenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var customers = await _customerService.GetAllAsync(tenantId, search, tag, skip, take);
            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customers");
            return StatusCode(500, new { message = "An error occurred while retrieving customers" });
        }
    }

    // GET: api/customers/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(Guid id)
    {
        try
        {
            var customer = await _customerService.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }

            return Ok(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer {CustomerId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the customer" });
        }
    }

    // POST: api/customers
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto dto)
    {
        try
        {
            // TODO: Get tenantId from authenticated user context
            var tenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var customer = await _customerService.CreateAsync(dto, tenantId);
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            return StatusCode(500, new { message = "An error occurred while creating the customer" });
        }
    }

    // PUT: api/customers/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomer(Guid id, UpdateCustomerDto dto)
    {
        try
        {
            var success = await _customerService.UpdateAsync(id, dto);

            if (!success)
            {
                return NotFound(new { message = "Customer not found" });
            }

            var updatedCustomer = await _customerService.GetByIdAsync(id);
            return Ok(updatedCustomer);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating customer {CustomerId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the customer" });
        }
    }

    // DELETE: api/customers/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(Guid id)
    {
        try
        {
            var success = await _customerService.DeleteAsync(id);

            if (!success)
            {
                return NotFound(new { message = "Customer not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting customer {CustomerId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the customer" });
        }
    }
}