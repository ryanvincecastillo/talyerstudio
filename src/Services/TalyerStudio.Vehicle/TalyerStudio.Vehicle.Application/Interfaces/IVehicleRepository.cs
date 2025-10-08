using TalyerStudio.Shared.Contracts.Vehicles;

namespace TalyerStudio.Vehicle.Application.Interfaces;

public interface IVehicleRepository
{
    Task<Domain.Entities.Vehicle?> GetByIdAsync(Guid id);
    Task<List<Domain.Entities.Vehicle>> GetAllAsync(Guid tenantId, Guid? customerId = null, string? search = null, int skip = 0, int take = 50);
    Task<Domain.Entities.Vehicle> CreateAsync(CreateVehicleDto dto, Guid tenantId);
    Task<bool> UpdateAsync(Guid id, UpdateVehicleDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsByPlateNumberAsync(string plateNumber, Guid tenantId, Guid? excludeVehicleId = null);
    Task<List<Domain.Entities.Vehicle>> GetByCustomerIdAsync(Guid customerId);
}