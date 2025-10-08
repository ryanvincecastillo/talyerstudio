using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalyerStudio.Vehicle.Infrastructure.Data;
using TalyerStudio.Shared.Contracts.Vehicles;

namespace TalyerStudio.Vehicle.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly VehicleDbContext _context;
    private readonly ILogger<VehiclesController> _logger;

    public VehiclesController(VehicleDbContext context, ILogger<VehiclesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/vehicles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehicles(
        [FromQuery] Guid? customerId = null,
        [FromQuery] string? search = null)
    {
        var query = _context.Vehicles.Where(v => v.DeletedAt == null);

        // Filter by customer if provided
        if (customerId.HasValue)
        {
            query = query.Where(v => v.CustomerId == customerId.Value);
        }

        // Search by plate number, make, or model
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(v => 
                v.PlateNumber.Contains(search) ||
                v.Make.Contains(search) ||
                v.Model.Contains(search));
        }

        var vehicles = await query
            .OrderByDescending(v => v.CreatedAt)
            .Take(50)
            .ToListAsync();

        var vehicleDtos = vehicles.Select(v => new VehicleDto
        {
            Id = v.Id,
            TenantId = v.TenantId,
            CustomerId = v.CustomerId,
            Make = v.Make,
            Model = v.Model,
            Year = v.Year,
            Color = v.Color,
            PlateNumber = v.PlateNumber,
            EngineNumber = v.EngineNumber,
            ChassisNumber = v.ChassisNumber,
            VehicleType = v.VehicleType.ToString(),
            VehicleCategory = v.VehicleCategory,
            Displacement = v.Displacement,
            FuelType = v.FuelType,
            Transmission = v.Transmission,
            CurrentOdometer = v.CurrentOdometer,
            OrCrExpiryDate = v.OrCrExpiryDate,
            TireSizeFront = v.TireSizeFront,
            TireSizeRear = v.TireSizeRear,
            Images = v.Images,
            QrCode = v.QrCode,
            Notes = v.Notes,
            CreatedAt = v.CreatedAt
        }).ToList();

        return Ok(vehicleDtos);
    }

    // GET: api/vehicles/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleDto>> GetVehicle(Guid id)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id && v.DeletedAt == null);

        if (vehicle == null)
        {
            return NotFound(new { message = "Vehicle not found" });
        }

        var vehicleDto = new VehicleDto
        {
            Id = vehicle.Id,
            TenantId = vehicle.TenantId,
            CustomerId = vehicle.CustomerId,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year,
            Color = vehicle.Color,
            PlateNumber = vehicle.PlateNumber,
            EngineNumber = vehicle.EngineNumber,
            ChassisNumber = vehicle.ChassisNumber,
            VehicleType = vehicle.VehicleType.ToString(),
            VehicleCategory = vehicle.VehicleCategory,
            Displacement = vehicle.Displacement,
            FuelType = vehicle.FuelType,
            Transmission = vehicle.Transmission,
            CurrentOdometer = vehicle.CurrentOdometer,
            OrCrExpiryDate = vehicle.OrCrExpiryDate,
            TireSizeFront = vehicle.TireSizeFront,
            TireSizeRear = vehicle.TireSizeRear,
            Images = vehicle.Images,
            QrCode = vehicle.QrCode,
            Notes = vehicle.Notes,
            CreatedAt = vehicle.CreatedAt
        };

        return Ok(vehicleDto);
    }

    // POST: api/vehicles
    [HttpPost]
    public async Task<ActionResult<VehicleDto>> CreateVehicle(CreateVehicleDto dto)
    {
        // TODO: Get tenantId from authenticated user context
        var tenantId = Guid.Parse("00000000-0000-0000-0000-000000000001"); // Temporary hardcoded

        // Parse VehicleType enum
        if (!Enum.TryParse<Domain.Entities.VehicleType>(dto.VehicleType, true, out var vehicleType))
        {
            return BadRequest(new { message = "Invalid vehicle type. Valid values: MOTORCYCLE, CAR, SUV, TRUCK, VAN, PICKUP" });
        }

        var vehicle = new Domain.Entities.Vehicle
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            CustomerId = dto.CustomerId,
            Make = dto.Make,
            Model = dto.Model,
            Year = dto.Year,
            Color = dto.Color,
            PlateNumber = dto.PlateNumber,
            EngineNumber = dto.EngineNumber,
            ChassisNumber = dto.ChassisNumber,
            VehicleType = vehicleType,
            VehicleCategory = dto.VehicleCategory,
            Displacement = dto.Displacement,
            FuelType = dto.FuelType,
            Transmission = dto.Transmission,
            CurrentOdometer = dto.CurrentOdometer,
            OrCrExpiryDate = dto.OrCrExpiryDate,
            TireSizeFront = dto.TireSizeFront,
            TireSizeRear = dto.TireSizeRear,
            Images = dto.Images,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        var vehicleDto = new VehicleDto
        {
            Id = vehicle.Id,
            TenantId = vehicle.TenantId,
            CustomerId = vehicle.CustomerId,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year,
            Color = vehicle.Color,
            PlateNumber = vehicle.PlateNumber,
            EngineNumber = vehicle.EngineNumber,
            ChassisNumber = vehicle.ChassisNumber,
            VehicleType = vehicle.VehicleType.ToString(),
            VehicleCategory = vehicle.VehicleCategory,
            Displacement = vehicle.Displacement,
            FuelType = vehicle.FuelType,
            Transmission = vehicle.Transmission,
            CurrentOdometer = vehicle.CurrentOdometer,
            OrCrExpiryDate = vehicle.OrCrExpiryDate,
            TireSizeFront = vehicle.TireSizeFront,
            TireSizeRear = vehicle.TireSizeRear,
            Images = vehicle.Images,
            QrCode = vehicle.QrCode,
            Notes = vehicle.Notes,
            CreatedAt = vehicle.CreatedAt
        };

        return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, vehicleDto);
    }

    // PUT: api/vehicles/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<VehicleDto>> UpdateVehicle(Guid id, UpdateVehicleDto dto)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id && v.DeletedAt == null);

        if (vehicle == null)
        {
            return NotFound(new { message = "Vehicle not found" });
        }

        // Parse VehicleType enum
        if (!Enum.TryParse<Domain.Entities.VehicleType>(dto.VehicleType, true, out var vehicleType))
        {
            return BadRequest(new { message = "Invalid vehicle type. Valid values: MOTORCYCLE, CAR, SUV, TRUCK, VAN, PICKUP" });
        }

        // Update fields
        vehicle.Make = dto.Make;
        vehicle.Model = dto.Model;
        vehicle.Year = dto.Year;
        vehicle.Color = dto.Color;
        vehicle.PlateNumber = dto.PlateNumber;
        vehicle.EngineNumber = dto.EngineNumber;
        vehicle.ChassisNumber = dto.ChassisNumber;
        vehicle.VehicleType = vehicleType;
        vehicle.VehicleCategory = dto.VehicleCategory;
        vehicle.Displacement = dto.Displacement;
        vehicle.FuelType = dto.FuelType;
        vehicle.Transmission = dto.Transmission;
        vehicle.CurrentOdometer = dto.CurrentOdometer;
        vehicle.OrCrExpiryDate = dto.OrCrExpiryDate;
        vehicle.TireSizeFront = dto.TireSizeFront;
        vehicle.TireSizeRear = dto.TireSizeRear;
        vehicle.Images = dto.Images;
        vehicle.Notes = dto.Notes;
        vehicle.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var vehicleDto = new VehicleDto
        {
            Id = vehicle.Id,
            TenantId = vehicle.TenantId,
            CustomerId = vehicle.CustomerId,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year,
            Color = vehicle.Color,
            PlateNumber = vehicle.PlateNumber,
            EngineNumber = vehicle.EngineNumber,
            ChassisNumber = vehicle.ChassisNumber,
            VehicleType = vehicle.VehicleType.ToString(),
            VehicleCategory = vehicle.VehicleCategory,
            Displacement = vehicle.Displacement,
            FuelType = vehicle.FuelType,
            Transmission = vehicle.Transmission,
            CurrentOdometer = vehicle.CurrentOdometer,
            OrCrExpiryDate = vehicle.OrCrExpiryDate,
            TireSizeFront = vehicle.TireSizeFront,
            TireSizeRear = vehicle.TireSizeRear,
            Images = vehicle.Images,
            QrCode = vehicle.QrCode,
            Notes = vehicle.Notes,
            CreatedAt = vehicle.CreatedAt
        };

        return Ok(vehicleDto);
    }

    // DELETE: api/vehicles/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteVehicle(Guid id)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id && v.DeletedAt == null);

        if (vehicle == null)
        {
            return NotFound(new { message = "Vehicle not found" });
        }

        // Soft delete
        vehicle.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/vehicles/customer/{customerId}
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehiclesByCustomer(Guid customerId)
    {
        var vehicles = await _context.Vehicles
            .Where(v => v.CustomerId == customerId && v.DeletedAt == null)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();

        var vehicleDtos = vehicles.Select(v => new VehicleDto
        {
            Id = v.Id,
            TenantId = v.TenantId,
            CustomerId = v.CustomerId,
            Make = v.Make,
            Model = v.Model,
            Year = v.Year,
            Color = v.Color,
            PlateNumber = v.PlateNumber,
            EngineNumber = v.EngineNumber,
            ChassisNumber = v.ChassisNumber,
            VehicleType = v.VehicleType.ToString(),
            VehicleCategory = v.VehicleCategory,
            Displacement = v.Displacement,
            FuelType = v.FuelType,
            Transmission = v.Transmission,
            CurrentOdometer = v.CurrentOdometer,
            OrCrExpiryDate = v.OrCrExpiryDate,
            TireSizeFront = v.TireSizeFront,
            TireSizeRear = v.TireSizeRear,
            Images = v.Images,
            QrCode = v.QrCode,
            Notes = v.Notes,
            CreatedAt = v.CreatedAt
        }).ToList();

        return Ok(vehicleDtos);
    }
}