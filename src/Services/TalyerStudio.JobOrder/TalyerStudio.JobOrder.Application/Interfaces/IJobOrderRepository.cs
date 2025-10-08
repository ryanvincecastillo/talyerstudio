using TalyerStudio.JobOrder.Application.DTOs;

namespace TalyerStudio.JobOrder.Application.Interfaces;

public interface IJobOrderRepository
{
    Task<Domain.Entities.JobOrder?> GetByIdAsync(Guid id);
    Task<List<Domain.Entities.JobOrder>> GetAllAsync(Guid tenantId, string? status = null, int skip = 0, int take = 50);
    Task<Domain.Entities.JobOrder> CreateAsync(CreateJobOrderDto dto, Guid tenantId);
    Task<bool> UpdateStatusAsync(Guid id, UpdateJobOrderStatusDto dto);
    Task<bool> AssignMechanicsAsync(Guid id, AssignMechanicDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<List<Domain.Entities.JobOrder>> GetByCustomerIdAsync(Guid customerId);
    Task<List<Domain.Entities.JobOrder>> GetByVehicleIdAsync(Guid vehicleId);
}