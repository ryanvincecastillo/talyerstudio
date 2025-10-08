using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalyerStudio.Customer.Infrastructure.Data;
using TalyerStudio.Shared.Contracts.Customers;

namespace TalyerStudio.Customer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly CustomerDbContext _context;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(CustomerDbContext context, ILogger<CustomersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers(
        [FromQuery] string? search = null,
        [FromQuery] string? tag = null)
    {
        var query = _context.Customers.Where(c => c.DeletedAt == null);

        // Search by name, email, or phone
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c => 
                c.FirstName.Contains(search) ||
                c.LastName.Contains(search) ||
                c.Email.Contains(search) ||
                c.PhoneNumber.Contains(search));
        }

        // Filter by tag
        if (!string.IsNullOrEmpty(tag))
        {
            query = query.Where(c => c.Tags.Contains(tag));
        }

        var customers = await query
            .OrderByDescending(c => c.CreatedAt)
            .Take(50)
            .ToListAsync();

        var customerDtos = customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            TenantId = c.TenantId,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email,
            PhoneNumber = c.PhoneNumber,
            Street = c.Street,
            Barangay = c.Barangay,
            Municipality = c.Municipality,
            Province = c.Province,
            ZipCode = c.ZipCode,
            Birthday = c.Birthday,
            LoyaltyPoints = c.LoyaltyPoints,
            CustomerType = c.CustomerType,
            Tags = c.Tags,
            Notes = c.Notes,
            CreatedAt = c.CreatedAt
        }).ToList();

        return Ok(customerDtos);
    }

    // GET: api/customers/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(Guid id)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (customer == null)
        {
            return NotFound(new { message = "Customer not found" });
        }

        var customerDto = new CustomerDto
        {
            Id = customer.Id,
            TenantId = customer.TenantId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Street = customer.Street,
            Barangay = customer.Barangay,
            Municipality = customer.Municipality,
            Province = customer.Province,
            ZipCode = customer.ZipCode,
            Birthday = customer.Birthday,
            LoyaltyPoints = customer.LoyaltyPoints,
            CustomerType = customer.CustomerType,
            Tags = customer.Tags,
            Notes = customer.Notes,
            CreatedAt = customer.CreatedAt
        };

        return Ok(customerDto);
    }

    // POST: api/customers
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto dto)
    {
        var customer = new Domain.Entities.Customer
        {
            Id = Guid.NewGuid(),
            TenantId = dto.TenantId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Street = dto.Street,
            Barangay = dto.Barangay,
            Municipality = dto.Municipality,
            Province = dto.Province,
            ZipCode = dto.ZipCode,
            Birthday = dto.Birthday,
            LoyaltyPoints = 0,
            CustomerType = dto.CustomerType,
            Tags = dto.Tags,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var customerDto = new CustomerDto
        {
            Id = customer.Id,
            TenantId = customer.TenantId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Street = customer.Street,
            Barangay = customer.Barangay,
            Municipality = customer.Municipality,
            Province = customer.Province,
            ZipCode = customer.ZipCode,
            Birthday = customer.Birthday,
            LoyaltyPoints = customer.LoyaltyPoints,
            CustomerType = customer.CustomerType,
            Tags = customer.Tags,
            Notes = customer.Notes,
            CreatedAt = customer.CreatedAt
        };

        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customerDto);
    }

    // PUT: api/customers/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomer(Guid id, UpdateCustomerDto dto)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (customer == null)
        {
            return NotFound(new { message = "Customer not found" });
        }

        // Update fields
        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.Email = dto.Email;
        customer.PhoneNumber = dto.PhoneNumber;
        customer.Street = dto.Street;
        customer.Barangay = dto.Barangay;
        customer.Municipality = dto.Municipality;
        customer.Province = dto.Province;
        customer.ZipCode = dto.ZipCode;
        customer.Birthday = dto.Birthday;
        customer.CustomerType = dto.CustomerType;
        customer.Tags = dto.Tags;
        customer.Notes = dto.Notes;
        customer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var customerDto = new CustomerDto
        {
            Id = customer.Id,
            TenantId = customer.TenantId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Street = customer.Street,
            Barangay = customer.Barangay,
            Municipality = customer.Municipality,
            Province = customer.Province,
            ZipCode = customer.ZipCode,
            Birthday = customer.Birthday,
            LoyaltyPoints = customer.LoyaltyPoints,
            CustomerType = customer.CustomerType,
            Tags = customer.Tags,
            Notes = customer.Notes,
            CreatedAt = customer.CreatedAt
        };

        return Ok(customerDto);
    }

    // DELETE: api/customers/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(Guid id)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (customer == null)
        {
            return NotFound(new { message = "Customer not found" });
        }

        // Soft delete
        customer.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}