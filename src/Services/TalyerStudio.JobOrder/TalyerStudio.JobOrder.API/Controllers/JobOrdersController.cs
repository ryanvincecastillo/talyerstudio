using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalyerStudio.JobOrder.Application.DTOs;
using TalyerStudio.JobOrder.Application.Interfaces;

namespace TalyerStudio.JobOrder.API.Controllers;

[ApiController]
[Route("api/job-orders")]
[Authorize]
public class JobOrdersController : ControllerBase
{
    private readonly IJobOrderService _jobOrderService;
    private readonly ILogger<JobOrdersController> _logger;

    public JobOrdersController(IJobOrderService jobOrderService, ILogger<JobOrdersController> logger)
    {
        _jobOrderService = jobOrderService;
        _logger = logger;
    }

    /// <summary>
    /// Get all job orders for a tenant
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<JobOrderDto>>> GetAll(
        [FromQuery] Guid? tenantId,
        [FromQuery] string? status,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        try
        {
            // TODO: Get tenantId from JWT token instead of query parameter
            var actualTenantId = tenantId ?? Guid.Parse("00000000-0000-0000-0000-000000000001");
            
            var jobOrders = await _jobOrderService.GetAllAsync(actualTenantId, status, skip, take);
            return Ok(jobOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting job orders");
            return StatusCode(500, "An error occurred while retrieving job orders");
        }
    }

    /// <summary>
    /// Get job order by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<JobOrderDto>> GetById(Guid id)
    {
        try
        {
            var jobOrder = await _jobOrderService.GetByIdAsync(id);
            
            if (jobOrder == null)
                return NotFound($"Job order with ID {id} not found");

            return Ok(jobOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting job order {JobOrderId}", id);
            return StatusCode(500, "An error occurred while retrieving the job order");
        }
    }

    /// <summary>
    /// Create a new job order
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<JobOrderDto>> Create(
        [FromBody] CreateJobOrderDto dto,
        [FromQuery] Guid? tenantId)
    {
        try
        {
            // TODO: Get tenantId from JWT token instead of query parameter
            var actualTenantId = tenantId ?? Guid.Parse("00000000-0000-0000-0000-000000000001");
            
            var jobOrder = await _jobOrderService.CreateAsync(dto, actualTenantId);
            return CreatedAtAction(nameof(GetById), new { id = jobOrder.Id }, jobOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating job order");
            return StatusCode(500, "An error occurred while creating the job order");
        }
    }

    /// <summary>
    /// Update job order status
    /// </summary>
    [HttpPatch("{id}/status")]
    public async Task<ActionResult> UpdateStatus(Guid id, [FromBody] UpdateJobOrderStatusDto dto)
    {
        try
        {
            var success = await _jobOrderService.UpdateStatusAsync(id, dto);
            
            if (!success)
                return NotFound($"Job order with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating job order status {JobOrderId}", id);
            return StatusCode(500, "An error occurred while updating the job order status");
        }
    }

    /// <summary>
    /// Assign mechanics to job order
    /// </summary>
    [HttpPost("{id}/assign")]
    public async Task<ActionResult> AssignMechanics(Guid id, [FromBody] AssignMechanicDto dto)
    {
        try
        {
            var success = await _jobOrderService.AssignMechanicsAsync(id, dto);
            
            if (!success)
                return NotFound($"Job order with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning mechanics to job order {JobOrderId}", id);
            return StatusCode(500, "An error occurred while assigning mechanics");
        }
    }

    /// <summary>
    /// Delete (soft delete) a job order
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var success = await _jobOrderService.DeleteAsync(id);
            
            if (!success)
                return NotFound($"Job order with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting job order {JobOrderId}", id);
            return StatusCode(500, "An error occurred while deleting the job order");
        }
    }

    /// <summary>
    /// Get job orders by customer ID
    /// </summary>
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<List<JobOrderDto>>> GetByCustomerId(Guid customerId)
    {
        try
        {
            var jobOrders = await _jobOrderService.GetByCustomerIdAsync(customerId);
            return Ok(jobOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting job orders for customer {CustomerId}", customerId);
            return StatusCode(500, "An error occurred while retrieving job orders");
        }
    }

    /// <summary>
    /// Get job orders by vehicle ID
    /// </summary>
    [HttpGet("vehicle/{vehicleId}")]
    public async Task<ActionResult<List<JobOrderDto>>> GetByVehicleId(Guid vehicleId)
    {
        try
        {
            var jobOrders = await _jobOrderService.GetByVehicleIdAsync(vehicleId);
            return Ok(jobOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting job orders for vehicle {VehicleId}", vehicleId);
            return StatusCode(500, "An error occurred while retrieving job orders");
        }
    }
}