using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalyerStudio.Customer.Application.Interfaces;
using TalyerStudio.Shared.Contracts.Services;

namespace TalyerStudio.Customer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceCategoriesController : ControllerBase
{
    private readonly IServiceCategoryService _serviceCategoryService;
    private readonly ILogger<ServiceCategoriesController> _logger;

    public ServiceCategoriesController(IServiceCategoryService serviceCategoryService, ILogger<ServiceCategoriesController> logger)
    {
        _serviceCategoryService = serviceCategoryService;
        _logger = logger;
    }

    // GET: api/servicecategories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceCategoryDto>>> GetServiceCategories([FromQuery] bool? isActive = null)
    {
        try
        {
            // TODO: Get tenantId from authenticated user context
            var tenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var categories = await _serviceCategoryService.GetAllAsync(tenantId, isActive);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving service categories");
            return StatusCode(500, new { message = "An error occurred while retrieving service categories" });
        }
    }

    // GET: api/servicecategories/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceCategoryDto>> GetServiceCategory(Guid id)
    {
        try
        {
            var category = await _serviceCategoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new { message = "Service category not found" });
            }

            return Ok(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving service category {CategoryId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the service category" });
        }
    }

    // POST: api/servicecategories
    [HttpPost]
    public async Task<ActionResult<ServiceCategoryDto>> CreateServiceCategory(CreateServiceCategoryDto dto)
    {
        try
        {
            // TODO: Get tenantId from authenticated user context
            var tenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var category = await _serviceCategoryService.CreateAsync(dto, tenantId);
            return CreatedAtAction(nameof(GetServiceCategory), new { id = category.Id }, category);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service category");
            return StatusCode(500, new { message = "An error occurred while creating the service category" });
        }
    }

    // PUT: api/servicecategories/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceCategoryDto>> UpdateServiceCategory(Guid id, UpdateServiceCategoryDto dto)
    {
        try
        {
            var success = await _serviceCategoryService.UpdateAsync(id, dto);

            if (!success)
            {
                return NotFound(new { message = "Service category not found" });
            }

            var updatedCategory = await _serviceCategoryService.GetByIdAsync(id);
            return Ok(updatedCategory);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating service category {CategoryId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the service category" });
        }
    }

    // DELETE: api/servicecategories/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteServiceCategory(Guid id)
    {
        try
        {
            var success = await _serviceCategoryService.DeleteAsync(id);

            if (!success)
            {
                return NotFound(new { message = "Service category not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting service category {CategoryId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the service category" });
        }
    }
}