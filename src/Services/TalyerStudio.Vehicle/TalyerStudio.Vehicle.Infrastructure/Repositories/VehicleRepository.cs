using Microsoft.EntityFrameworkCore;
using TalyerStudio.Vehicle.Application.Interfaces;
using TalyerStudio.Vehicle.Infrastructure.Data;
using TalyerStudio.Shared.Contracts.Vehicles;

namespace TalyerStudio.Vehicle.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly VehicleDbContext _context;

    public VehicleRepository(VehicleDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Vehicle?> GetByIdAsync(Guid id)
    {
        return await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id && v.DeletedAt == null);
    }

    public async Task<List<Domain.Entities.Vehicle>> GetAllAsync(Guid tenantId, Guid? customerId = null, string? search = null, int skip = 0, int take = 50)
    {
        var query = _context.Vehicles.Where(v => v.TenantId == tenantId && v.DeletedAt == null);

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

        return await query
            .OrderByDescending(v => v.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<Domain.Entities.Vehicle> CreateAsync(CreateVehicleDto dto, Guid tenantId)
    {
        // Parse VehicleType enum
        if (!Enum.TryParse<Domain.Entities.VehicleType>(dto.VehicleType, true, out var vehicleType))
        {
            throw new ArgumentException("Invalid vehicle type. Valid values: MOTORCYCLE, CAR, SUV, TRUCK, VAN, PICKUP");
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
            Images = dto.Images ?? Array.Empty<string>(),
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        return vehicle;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateVehicleDto dto)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id && v.DeletedAt == null);

        if (vehicle == null)
        {
            return false;
        }

        // Parse VehicleType enum
        if (!Enum.TryParse<Domain.Entities.VehicleType>(dto.VehicleType, true, out var vehicleType))
        {
            throw new ArgumentException("Invalid vehicle type. Valid values: MOTORCYCLE, CAR, SUV, TRUCK, VAN, PICKUP");
        }

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
        vehicle.Images = dto.Images ?? vehicle.Images;
        vehicle.Notes = dto.Notes;
        vehicle.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id && v.DeletedAt == null);

        if (vehicle == null)
        {
            return false;
        }

        vehicle.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsByPlateNumberAsync(string plateNumber, Guid tenantId, Guid? excludeVehicleId = null)
    {
        var query = _context.Vehicles
            .Where(v => v.TenantId == tenantId && v.PlateNumber == plateNumber && v.DeletedAt == null);

        if (excludeVehicleId.HasValue)
        {
            query = query.Where(v => v.Id != excludeVehicleId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<List<Domain.Entities.Vehicle>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Vehicles
            .Where(v => v.CustomerId == customerId && v.DeletedAt == null)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();
    }
}