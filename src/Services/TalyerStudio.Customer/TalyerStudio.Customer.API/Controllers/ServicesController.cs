using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalyerStudio.Customer.Infrastructure.Data;
using TalyerStudio.Customer.Domain.Entities;
using TalyerStudio.Shared.Contracts.Services;

namespace TalyerStudio.Customer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly CustomerDbContext _context;
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(CustomerDbContext context, ILogger<ServicesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/services
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServices(
        [FromQuery] Guid? categoryId = null,
        [FromQuery] string? applicability = null,
        [FromQuery] bool? isActive = null)
    {
        var query = _context.Services
            .Include(s => s.Category)
            .Where(s => s.DeletedAt == null);

        // Filter by category
        if (categoryId.HasValue)
        {
            query = query.Where(s => s.CategoryId == categoryId.Value);
        }

        // Filter by applicability
        if (!string.IsNullOrEmpty(applicability))
        {
            if (Enum.TryParse<ServiceApplicability>(applicability, true, out var app))
            {
                query = query.Where(s => s.Applicability == app || s.Applicability == ServiceApplicability.BOTH);
            }
        }

        // Filter by active status
        if (isActive.HasValue)
        {
            query = query.Where(s => s.IsActive == isActive.Value);
        }

        var services = await query
            .OrderBy(s => s.DisplayOrder)
            .ThenBy(s => s.Name)
            .ToListAsync();

        var serviceDtos = services.Select(s => new ServiceDto
        {
            Id = s.Id,
            TenantId = s.TenantId,
            Name = s.Name,
            Description = s.Description,
            CategoryId = s.CategoryId,
            CategoryName = s.Category?.Name,
            BasePrice = s.BasePrice,
            Currency = s.Currency,
            Applicability = s.Applicability.ToString(),
            EstimatedDurationMinutes = s.EstimatedDurationMinutes,
            IsActive = s.IsActive,
            DisplayOrder = s.DisplayOrder,
            Icon = s.Icon,
            CreatedAt = s.CreatedAt
        }).ToList();

        return Ok(serviceDtos);
    }

    // GET: api/services/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceDto>> GetService(Guid id)
    {
        var service = await _context.Services
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (service == null)
        {
            return NotFound(new { message = "Service not found" });
        }

        var serviceDto = new ServiceDto
        {
            Id = service.Id,
            TenantId = service.TenantId,
            Name = service.Name,
            Description = service.Description,
            CategoryId = service.CategoryId,
            CategoryName = service.Category?.Name,
            BasePrice = service.BasePrice,
            Currency = service.Currency,
            Applicability = service.Applicability.ToString(),
            EstimatedDurationMinutes = service.EstimatedDurationMinutes,
            IsActive = service.IsActive,
            DisplayOrder = service.DisplayOrder,
            Icon = service.Icon,
            CreatedAt = service.CreatedAt
        };

        return Ok(serviceDto);
    }

    // POST: api/services
    [HttpPost]
    public async Task<ActionResult<ServiceDto>> CreateService(CreateServiceDto dto)
    {
        // Validate category exists
        var categoryExists = await _context.ServiceCategories
            .AnyAsync(c => c.Id == dto.CategoryId && c.DeletedAt == null);

        if (!categoryExists)
        {
            return BadRequest(new { message = "Invalid category ID" });
        }

        // Parse applicability
        if (!Enum.TryParse<ServiceApplicability>(dto.Applicability, true, out var applicability))
        {
            return BadRequest(new { message = "Invalid applicability. Valid values: AUTO, MOTORCYCLE, BOTH" });
        }

        // TODO: Get tenantId from authenticated user context
        var tenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var service = new Service
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = dto.Name,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            BasePrice = dto.BasePrice,
            Currency = "PHP",
            Applicability = applicability,
            EstimatedDurationMinutes = dto.EstimatedDurationMinutes,
            IsActive = true,
            DisplayOrder = dto.DisplayOrder,
            Icon = dto.Icon,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        // Reload with category
        service = await _context.Services
            .Include(s => s.Category)
            .FirstAsync(s => s.Id == service.Id);

        var serviceDto = new ServiceDto
        {
            Id = service.Id,
            TenantId = service.TenantId,
            Name = service.Name,
            Description = service.Description,
            CategoryId = service.CategoryId,
            CategoryName = service.Category?.Name,
            BasePrice = service.BasePrice,
            Currency = service.Currency,
            Applicability = service.Applicability.ToString(),
            EstimatedDurationMinutes = service.EstimatedDurationMinutes,
            IsActive = service.IsActive,
            DisplayOrder = service.DisplayOrder,
            Icon = service.Icon,
            CreatedAt = service.CreatedAt
        };

        return CreatedAtAction(nameof(GetService), new { id = service.Id }, serviceDto);
    }

    // PUT: api/services/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceDto>> UpdateService(Guid id, UpdateServiceDto dto)
    {
        var service = await _context.Services
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (service == null)
        {
            return NotFound(new { message = "Service not found" });
        }

        // Validate category exists
        var categoryExists = await _context.ServiceCategories
            .AnyAsync(c => c.Id == dto.CategoryId && c.DeletedAt == null);

        if (!categoryExists)
        {
            return BadRequest(new { message = "Invalid category ID" });
        }

        // Parse applicability
        if (!Enum.TryParse<ServiceApplicability>(dto.Applicability, true, out var applicability))
        {
            return BadRequest(new { message = "Invalid applicability. Valid values: AUTO, MOTORCYCLE, BOTH" });
        }

        service.Name = dto.Name;
        service.Description = dto.Description;
        service.CategoryId = dto.CategoryId;
        service.BasePrice = dto.BasePrice;
        service.Applicability = applicability;
        service.EstimatedDurationMinutes = dto.EstimatedDurationMinutes;
        service.IsActive = dto.IsActive;
        service.DisplayOrder = dto.DisplayOrder;
        service.Icon = dto.Icon;
        service.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Reload category
        service = await _context.Services
            .Include(s => s.Category)
            .FirstAsync(s => s.Id == id);

        var serviceDto = new ServiceDto
        {
            Id = service.Id,
            TenantId = service.TenantId,
            Name = service.Name,
            Description = service.Description,
            CategoryId = service.CategoryId,
            CategoryName = service.Category?.Name,
            BasePrice = service.BasePrice,
            Currency = service.Currency,
            Applicability = service.Applicability.ToString(),
            EstimatedDurationMinutes = service.EstimatedDurationMinutes,
            IsActive = service.IsActive,
            DisplayOrder = service.DisplayOrder,
            Icon = service.Icon,
            CreatedAt = service.CreatedAt
        };

        return Ok(serviceDto);
    }

    // DELETE: api/services/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteService(Guid id)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (service == null)
        {
            return NotFound(new { message = "Service not found" });
        }

        // Soft delete
        service.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}