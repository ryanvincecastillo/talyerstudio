using Microsoft.AspNetCore.Mvc;
using TalyerStudio.Inventory.Application.DTOs;
using TalyerStudio.Inventory.Application.Interfaces;

namespace TalyerStudio.Inventory.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll(
        [FromQuery] Guid? tenantId,
        [FromQuery] Guid? categoryId,
        [FromQuery] string? search,
        [FromQuery] bool? isActive,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        try
        {
            // TODO: Get tenantId from JWT token
            var actualTenantId = tenantId ?? Guid.Parse("00000000-0000-0000-0000-000000000001");
            
            var products = await _productService.GetAllAsync(actualTenantId, categoryId, search, isActive, skip, take);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products");
            return StatusCode(500, "An error occurred while retrieving products");
        }
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        try
        {
            var product = await _productService.GetByIdAsync(id);
            
            if (product == null)
                return NotFound($"Product with ID {id} not found");

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product {ProductId}", id);
            return StatusCode(500, "An error occurred while retrieving the product");
        }
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(
        [FromBody] CreateProductDto dto,
        [FromQuery] Guid? tenantId)
    {
        try
        {
            // TODO: Get tenantId from JWT token
            var actualTenantId = tenantId ?? Guid.Parse("00000000-0000-0000-0000-000000000001");
            
            var product = await _productService.CreateAsync(dto, actualTenantId);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            return StatusCode(500, "An error occurred while creating the product");
        }
    }

    /// <summary>
    /// Update a product
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        try
        {
            var success = await _productService.UpdateAsync(id, dto);
            
            if (!success)
                return NotFound($"Product with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product {ProductId}", id);
            return StatusCode(500, "An error occurred while updating the product");
        }
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var success = await _productService.DeleteAsync(id);
            
            if (!success)
                return NotFound($"Product with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product {ProductId}", id);
            return StatusCode(500, "An error occurred while deleting the product");
        }
    }

    /// <summary>
    /// Get low stock products
    /// </summary>
    [HttpGet("low-stock")]
    public async Task<ActionResult<List<ProductDto>>> GetLowStock([FromQuery] Guid? tenantId)
    {
        try
        {
            // TODO: Get tenantId from JWT token
            var actualTenantId = tenantId ?? Guid.Parse("00000000-0000-0000-0000-000000000001");
            
            var products = await _productService.GetLowStockProductsAsync(actualTenantId);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting low stock products");
            return StatusCode(500, "An error occurred while retrieving low stock products");
        }
    }
}