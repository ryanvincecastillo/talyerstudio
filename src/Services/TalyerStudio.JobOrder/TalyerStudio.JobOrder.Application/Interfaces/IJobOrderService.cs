using TalyerStudio.JobOrder.Application.DTOs;

namespace TalyerStudio.JobOrder.Application.Interfaces;

public interface IJobOrderService
{
    Task<JobOrderDto?> GetByIdAsync(Guid id);
    Task<List<JobOrderDto>> GetAllAsync(Guid tenantId, string? status = null, int skip = 0, int take = 50);
    Task<JobOrderDto> CreateAsync(CreateJobOrderDto dto, Guid tenantId);
    Task<bool> UpdateStatusAsync(Guid id, UpdateJobOrderStatusDto dto);
    Task<bool> AssignMechanicsAsync(Guid id, AssignMechanicDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<List<JobOrderDto>> GetByCustomerIdAsync(Guid customerId);
    Task<List<JobOrderDto>> GetByVehicleIdAsync(Guid vehicleId);
}