using TalyerStudio.Vehicle.Application.Interfaces;
using TalyerStudio.Shared.Contracts.Vehicles;

namespace TalyerStudio.Vehicle.Application.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _repository;

    public VehicleService(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<VehicleDto?> GetByIdAsync(Guid id)
    {
        var vehicle = await _repository.GetByIdAsync(id);
        return vehicle == null ? null : MapToDto(vehicle);
    }

    public async Task<List<VehicleDto>> GetAllAsync(Guid tenantId, Guid? customerId = null, string? search = null, int skip = 0, int take = 50)
    {
        var vehicles = await _repository.GetAllAsync(tenantId, customerId, search, skip, take);
        return vehicles.Select(MapToDto).ToList();
    }

    public async Task<VehicleDto> CreateAsync(CreateVehicleDto dto, Guid tenantId)
    {
        // Validate plate number uniqueness
        if (!string.IsNullOrEmpty(dto.PlateNumber))
        {
            var plateExists = await _repository.ExistsByPlateNumberAsync(dto.PlateNumber, tenantId);
            if (plateExists)
            {
                throw new InvalidOperationException($"Vehicle with plate number '{dto.PlateNumber}' already exists");
            }
        }

        var vehicle = await _repository.CreateAsync(dto, tenantId);
        return MapToDto(vehicle);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateVehicleDto dto)
    {
        // Get existing vehicle
        var existingVehicle = await _repository.GetByIdAsync(id);
        if (existingVehicle == null)
        {
            throw new InvalidOperationException("Vehicle not found");
        }

        // Validate plate number uniqueness (if changed)
        if (!string.IsNullOrEmpty(dto.PlateNumber) && dto.PlateNumber != existingVehicle.PlateNumber)
        {
            var plateExists = await _repository.ExistsByPlateNumberAsync(dto.PlateNumber, existingVehicle.TenantId, id);
            if (plateExists)
            {
                throw new InvalidOperationException($"Vehicle with plate number '{dto.PlateNumber}' already exists");
            }
        }

        return await _repository.UpdateAsync(id, dto);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<List<VehicleDto>> GetByCustomerIdAsync(Guid customerId)
    {
        var vehicles = await _repository.GetByCustomerIdAsync(customerId);
        return vehicles.Select(MapToDto).ToList();
    }

    private VehicleDto MapToDto(Domain.Entities.Vehicle vehicle)
    {
        return new VehicleDto
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
    }
}