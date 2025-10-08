using TalyerStudio.JobOrder.Application.DTOs;
using TalyerStudio.JobOrder.Application.Interfaces;

namespace TalyerStudio.JobOrder.Application.Services;

public class JobOrderService : IJobOrderService
{
    private readonly IJobOrderRepository _repository;

    public JobOrderService(IJobOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<JobOrderDto?> GetByIdAsync(Guid id)
    {
        var jobOrder = await _repository.GetByIdAsync(id);
        return jobOrder == null ? null : MapToDto(jobOrder);
    }

    public async Task<List<JobOrderDto>> GetAllAsync(Guid tenantId, string? status = null, int skip = 0, int take = 50)
    {
        var jobOrders = await _repository.GetAllAsync(tenantId, status, skip, take);
        return jobOrders.Select(MapToDto).ToList();
    }

    public async Task<JobOrderDto> CreateAsync(CreateJobOrderDto dto, Guid tenantId)
    {
        var jobOrder = await _repository.CreateAsync(dto, tenantId);
        return MapToDto(jobOrder);
    }

    public async Task<bool> UpdateStatusAsync(Guid id, UpdateJobOrderStatusDto dto)
    {
        return await _repository.UpdateStatusAsync(id, dto);
    }

    public async Task<bool> AssignMechanicsAsync(Guid id, AssignMechanicDto dto)
    {
        return await _repository.AssignMechanicsAsync(id, dto);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<List<JobOrderDto>> GetByCustomerIdAsync(Guid customerId)
    {
        var jobOrders = await _repository.GetByCustomerIdAsync(customerId);
        return jobOrders.Select(MapToDto).ToList();
    }

    public async Task<List<JobOrderDto>> GetByVehicleIdAsync(Guid vehicleId)
    {
        var jobOrders = await _repository.GetByVehicleIdAsync(vehicleId);
        return jobOrders.Select(MapToDto).ToList();
    }

    private JobOrderDto MapToDto(Domain.Entities.JobOrder jobOrder)
    {
        return new JobOrderDto
        {
            Id = jobOrder.Id,
            JobOrderNumber = jobOrder.JobOrderNumber,
            TenantId = jobOrder.TenantId,
            BranchId = jobOrder.BranchId,
            CustomerId = jobOrder.CustomerId,
            VehicleId = jobOrder.VehicleId,
            Status = jobOrder.Status.ToString(),
            Priority = jobOrder.Priority.ToString(),
            OdometerReading = jobOrder.OdometerReading,
            CustomerComplaints = jobOrder.CustomerComplaints,
            InspectionNotes = jobOrder.InspectionNotes,
            BeforePhotos = jobOrder.BeforePhotos,
            AfterPhotos = jobOrder.AfterPhotos,
            AssignedMechanicIds = jobOrder.AssignedMechanicIds,
            StartTime = jobOrder.StartTime,
            EndTime = jobOrder.EndTime,
            EstimatedCompletionTime = jobOrder.EstimatedCompletionTime,
            TotalAmount = jobOrder.TotalAmount,
            DiscountAmount = jobOrder.DiscountAmount,
            TaxAmount = jobOrder.TaxAmount,
            GrandTotal = jobOrder.GrandTotal,
            Items = jobOrder.Items.Select(i => new JobOrderItemDto
            {
                Id = i.Id,
                JobOrderId = i.JobOrderId,
                ServiceId = i.ServiceId,
                ServiceName = i.ServiceName,
                Description = i.Description,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Subtotal = i.Subtotal,
                Notes = i.Notes
            }).ToList(),
            Parts = jobOrder.Parts.Select(p => new JobOrderPartDto
            {
                Id = p.Id,
                JobOrderId = p.JobOrderId,
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductSku = p.ProductSku,
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice,
                Subtotal = p.Subtotal,
                Notes = p.Notes
            }).ToList(),
            CreatedAt = jobOrder.CreatedAt,
            UpdatedAt = jobOrder.UpdatedAt
        };
    }
}