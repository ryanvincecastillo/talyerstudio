using Microsoft.AspNetCore.Mvc;
using TalyerStudio.Customer.Application.Interfaces;
using TalyerStudio.Shared.Contracts.Services;

namespace TalyerStudio.Customer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(IServiceService serviceService, ILogger<ServicesController> logger)
    {
        _serviceService = serviceService;
        _logger = logger;
    }

    // GET: api/services
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServices(
        [FromQuery] Guid? categoryId = null,
        [FromQuery] string? applicability = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        try
        {
            // TODO: Get tenantId from authenticated user context
            var tenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var services = await _serviceService.GetAllAsync(tenantId, categoryId, applicability, isActive, skip, take);
            return Ok(services);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving services");
            return StatusCode(500, new { message = "An error occurred while retrieving services" });
        }
    }

    // GET: api/services/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceDto>> GetService(Guid id)
    {
        try
        {
            var service = await _serviceService.GetByIdAsync(id);

            if (service == null)
            {
                return NotFound(new { message = "Service not found" });
            }

            return Ok(service);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving service {ServiceId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the service" });
        }
    }

    // GET: api/services/category/{categoryId}
    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServicesByCategory(Guid categoryId)
    {
        try
        {
            var services = await _serviceService.GetByCategoryIdAsync(categoryId);
            return Ok(services);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving services for category {CategoryId}", categoryId);
            return StatusCode(500, new { message = "An error occurred while retrieving services" });
        }
    }

    // POST: api/services
    [HttpPost]
    public async Task<ActionResult<ServiceDto>> CreateService(CreateServiceDto dto)
    {
        try
        {
            // TODO: Get tenantId from authenticated user context
            var tenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var service = await _serviceService.CreateAsync(dto, tenantId);
            return CreatedAtAction(nameof(GetService), new { id = service.Id }, service);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service");
            return StatusCode(500, new { message = "An error occurred while creating the service" });
        }
    }

    // PUT: api/services/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceDto>> UpdateService(Guid id, UpdateServiceDto dto)
    {
        try
        {
            var success = await _serviceService.UpdateAsync(id, dto);

            if (!success)
            {
                return NotFound(new { message = "Service not found" });
            }

            var updatedService = await _serviceService.GetByIdAsync(id);
            return Ok(updatedService);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating service {ServiceId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the service" });
        }
    }

    // DELETE: api/services/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteService(Guid id)
    {
        try
        {
            var success = await _serviceService.DeleteAsync(id);

            if (!success)
            {
                return NotFound(new { message = "Service not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting service {ServiceId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the service" });
        }
    }
}