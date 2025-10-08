using Microsoft.AspNetCore.Mvc;
using TalyerStudio.Vehicle.Application.Interfaces;
using TalyerStudio.Shared.Contracts.Vehicles;

namespace TalyerStudio.Vehicle.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    private readonly ILogger<VehiclesController> _logger;

    public VehiclesController(IVehicleService vehicleService, ILogger<VehiclesController> logger)
    {
        _vehicleService = vehicleService;
        _logger = logger;
    }

    // GET: api/vehicles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehicles(
        [FromQuery] Guid? customerId = null,
        [FromQuery] string? search = null,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        try
        {
            // TODO: Get tenantId from authenticated user context
            var tenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var vehicles = await _vehicleService.GetAllAsync(tenantId, customerId, search, skip, take);
            return Ok(vehicles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vehicles");
            return StatusCode(500, new { message = "An error occurred while retrieving vehicles" });
        }
    }

    // GET: api/vehicles/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleDto>> GetVehicle(Guid id)
    {
        try
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);

            if (vehicle == null)
            {
                return NotFound(new { message = "Vehicle not found" });
            }

            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vehicle {VehicleId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the vehicle" });
        }
    }

    // GET: api/vehicles/customer/{customerId}
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehiclesByCustomer(Guid customerId)
    {
        try
        {
            var vehicles = await _vehicleService.GetByCustomerIdAsync(customerId);
            return Ok(vehicles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vehicles for customer {CustomerId}", customerId);
            return StatusCode(500, new { message = "An error occurred while retrieving vehicles" });
        }
    }

    // POST: api/vehicles
    [HttpPost]
    public async Task<ActionResult<VehicleDto>> CreateVehicle(CreateVehicleDto dto)
    {
        try
        {
            // TODO: Get tenantId from authenticated user context
            var tenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var vehicle = await _vehicleService.CreateAsync(dto, tenantId);
            return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, vehicle);
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
            _logger.LogError(ex, "Error creating vehicle");
            return StatusCode(500, new { message = "An error occurred while creating the vehicle" });
        }
    }

    // PUT: api/vehicles/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<VehicleDto>> UpdateVehicle(Guid id, UpdateVehicleDto dto)
    {
        try
        {
            var success = await _vehicleService.UpdateAsync(id, dto);

            if (!success)
            {
                return NotFound(new { message = "Vehicle not found" });
            }

            var updatedVehicle = await _vehicleService.GetByIdAsync(id);
            return Ok(updatedVehicle);
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
            _logger.LogError(ex, "Error updating vehicle {VehicleId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the vehicle" });
        }
    }

    // DELETE: api/vehicles/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteVehicle(Guid id)
    {
        try
        {
            var success = await _vehicleService.DeleteAsync(id);

            if (!success)
            {
                return NotFound(new { message = "Vehicle not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting vehicle {VehicleId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the vehicle" });
        }
    }
}