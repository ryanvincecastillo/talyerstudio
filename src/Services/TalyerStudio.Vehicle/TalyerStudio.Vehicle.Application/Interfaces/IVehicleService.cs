using TalyerStudio.Shared.Contracts.Vehicles;

namespace TalyerStudio.Vehicle.Application.Interfaces;

public interface IVehicleService
{
    Task<VehicleDto?> GetByIdAsync(Guid id);
    Task<List<VehicleDto>> GetAllAsync(Guid tenantId, Guid? customerId = null, string? search = null, int skip = 0, int take = 50);
    Task<VehicleDto> CreateAsync(CreateVehicleDto dto, Guid tenantId);
    Task<bool> UpdateAsync(Guid id, UpdateVehicleDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<List<VehicleDto>> GetByCustomerIdAsync(Guid customerId);
}