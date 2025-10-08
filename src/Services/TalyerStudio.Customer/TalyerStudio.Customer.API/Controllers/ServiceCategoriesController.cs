using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalyerStudio.Customer.Infrastructure.Data;
using TalyerStudio.Customer.Domain.Entities;
using TalyerStudio.Shared.Contracts.Services;

namespace TalyerStudio.Customer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceCategoriesController : ControllerBase
{
    private readonly CustomerDbContext _context;
    private readonly ILogger<ServiceCategoriesController> _logger;

    public ServiceCategoriesController(CustomerDbContext context, ILogger<ServiceCategoriesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/servicecategories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceCategoryDto>>> GetServiceCategories()
    {
        var categories = await _context.ServiceCategories
            .Where(c => c.DeletedAt == null)
            .Include(c => c.Services)
            .OrderBy(c => c.DisplayOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();

        var categoryDtos = categories.Select(c => new ServiceCategoryDto
        {
            Id = c.Id,
            TenantId = c.TenantId,
            Name = c.Name,
            Description = c.Description,
            DisplayOrder = c.DisplayOrder,
            Icon = c.Icon,
            Color = c.Color,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt,
            ServiceCount = c.Services.Count(s => s.DeletedAt == null)
        }).ToList();

        return Ok(categoryDtos);
    }

    // GET: api/servicecategories/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceCategoryDto>> GetServiceCategory(Guid id)
    {
        var category = await _context.ServiceCategories
            .Include(c => c.Services)
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (category == null)
        {
            return NotFound(new { message = "Service category not found" });
        }

        var categoryDto = new ServiceCategoryDto
        {
            Id = category.Id,
            TenantId = category.TenantId,
            Name = category.Name,
            Description = category.Description,
            DisplayOrder = category.DisplayOrder,
            Icon = category.Icon,
            Color = category.Color,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            ServiceCount = category.Services.Count(s => s.DeletedAt == null)
        };

        return Ok(categoryDto);
    }

    // POST: api/servicecategories
    [HttpPost]
    public async Task<ActionResult<ServiceCategoryDto>> CreateServiceCategory(CreateServiceCategoryDto dto)
    {
        // TODO: Get tenantId from authenticated user context
        var tenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var category = new ServiceCategory
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = dto.Name,
            Description = dto.Description,
            DisplayOrder = dto.DisplayOrder,
            Icon = dto.Icon,
            Color = dto.Color,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.ServiceCategories.Add(category);
        await _context.SaveChangesAsync();

        var categoryDto = new ServiceCategoryDto
        {
            Id = category.Id,
            TenantId = category.TenantId,
            Name = category.Name,
            Description = category.Description,
            DisplayOrder = category.DisplayOrder,
            Icon = category.Icon,
            Color = category.Color,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            ServiceCount = 0
        };

        return CreatedAtAction(nameof(GetServiceCategory), new { id = category.Id }, categoryDto);
    }

    // PUT: api/servicecategories/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceCategoryDto>> UpdateServiceCategory(Guid id, UpdateServiceCategoryDto dto)
    {
        var category = await _context.ServiceCategories
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (category == null)
        {
            return NotFound(new { message = "Service category not found" });
        }

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.DisplayOrder = dto.DisplayOrder;
        category.Icon = dto.Icon;
        category.Color = dto.Color;
        category.IsActive = dto.IsActive;
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var categoryDto = new ServiceCategoryDto
        {
            Id = category.Id,
            TenantId = category.TenantId,
            Name = category.Name,
            Description = category.Description,
            DisplayOrder = category.DisplayOrder,
            Icon = category.Icon,
            Color = category.Color,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            ServiceCount = await _context.Services.CountAsync(s => s.CategoryId == id && s.DeletedAt == null)
        };

        return Ok(categoryDto);
    }

    // DELETE: api/servicecategories/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteServiceCategory(Guid id)
    {
        var category = await _context.ServiceCategories
            .Include(c => c.Services)
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (category == null)
        {
            return NotFound(new { message = "Service category not found" });
        }

        // Check if category has services
        var activeServices = category.Services.Count(s => s.DeletedAt == null);
        if (activeServices > 0)
        {
            return BadRequest(new { message = $"Cannot delete category with {activeServices} active service(s). Delete or reassign services first." });
        }

        // Soft delete
        category.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}