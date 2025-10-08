using Microsoft.AspNetCore.Mvc;
using TalyerStudio.Inventory.Application.DTOs;
using TalyerStudio.Inventory.Application.Interfaces;

namespace TalyerStudio.Inventory.API.Controllers;

[ApiController]
[Route("api/product-categories")]
public class ProductCategoriesController : ControllerBase
{
    private readonly IProductCategoryService _categoryService;
    private readonly ILogger<ProductCategoriesController> _logger;

    public ProductCategoriesController(IProductCategoryService categoryService, ILogger<ProductCategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <summary>
    /// Get all product categories
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ProductCategoryDto>>> GetAll(
        [FromQuery] Guid? tenantId,
        [FromQuery] bool? isActive)
    {
        try
        {
            // TODO: Get tenantId from JWT token
            var actualTenantId = tenantId ?? Guid.Parse("00000000-0000-0000-0000-000000000001");
            
            var categories = await _categoryService.GetAllAsync(actualTenantId, isActive);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product categories");
            return StatusCode(500, "An error occurred while retrieving product categories");
        }
    }

    /// <summary>
    /// Get product category by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductCategoryDto>> GetById(Guid id)
    {
        try
        {
            var category = await _categoryService.GetByIdAsync(id);
            
            if (category == null)
                return NotFound($"Product category with ID {id} not found");

            return Ok(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product category {CategoryId}", id);
            return StatusCode(500, "An error occurred while retrieving the product category");
        }
    }

    /// <summary>
    /// Create a new product category
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProductCategoryDto>> Create(
        [FromBody] CreateProductCategoryDto dto,
        [FromQuery] Guid? tenantId)
    {
        try
        {
            // TODO: Get tenantId from JWT token
            var actualTenantId = tenantId ?? Guid.Parse("00000000-0000-0000-0000-000000000001");
            
            var category = await _categoryService.CreateAsync(dto, actualTenantId);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product category");
            return StatusCode(500, "An error occurred while creating the product category");
        }
    }

    /// <summary>
    /// Update a product category
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] CreateProductCategoryDto dto)
    {
        try
        {
            var success = await _categoryService.UpdateAsync(id, dto);
            
            if (!success)
                return NotFound($"Product category with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product category {CategoryId}", id);
            return StatusCode(500, "An error occurred while updating the product category");
        }
    }

    /// <summary>
    /// Delete a product category
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var success = await _categoryService.DeleteAsync(id);
            
            if (!success)
                return NotFound($"Product category with ID {id} not found or has products");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product category {CategoryId}", id);
            return StatusCode(500, "An error occurred while deleting the product category");
        }
    }
}