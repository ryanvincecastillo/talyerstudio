using Microsoft.EntityFrameworkCore;
using TalyerStudio.JobOrder.Application.DTOs;
using TalyerStudio.JobOrder.Application.Interfaces;
using TalyerStudio.JobOrder.Domain.Enums;
using TalyerStudio.JobOrder.Infrastructure.Data;

namespace TalyerStudio.JobOrder.Infrastructure.Repositories;

public class JobOrderRepository : IJobOrderRepository
{
    private readonly JobOrderDbContext _context;

    public JobOrderRepository(JobOrderDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.JobOrder?> GetByIdAsync(Guid id)
    {
        return await _context.JobOrders
            .Include(jo => jo.Items)
            .Include(jo => jo.Parts)
            .FirstOrDefaultAsync(jo => jo.Id == id && jo.DeletedAt == null);
    }

    public async Task<List<Domain.Entities.JobOrder>> GetAllAsync(Guid tenantId, string? status = null, int skip = 0, int take = 50)
    {
        var query = _context.JobOrders
            .Include(jo => jo.Items)
            .Include(jo => jo.Parts)
            .Where(jo => jo.TenantId == tenantId && jo.DeletedAt == null);

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(jo => jo.Status.ToString() == status.ToUpper());
        }

        return await query
            .OrderByDescending(jo => jo.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<Domain.Entities.JobOrder> CreateAsync(CreateJobOrderDto dto, Guid tenantId)
    {
        var jobOrder = new Domain.Entities.JobOrder
        {
            Id = Guid.NewGuid(),
            JobOrderNumber = await GenerateJobOrderNumberAsync(tenantId),
            TenantId = tenantId,
            CustomerId = dto.CustomerId,
            VehicleId = dto.VehicleId,
            Priority = Enum.Parse<JobOrderPriority>(dto.Priority.ToUpper()),
            Status = JobOrderStatus.PENDING,
            OdometerReading = dto.OdometerReading,
            CustomerComplaints = dto.CustomerComplaints,
            InspectionNotes = dto.InspectionNotes ?? string.Empty,
            EstimatedCompletionTime = dto.EstimatedCompletionTime,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Add items
        foreach (var itemDto in dto.Items)
        {
            var item = new Domain.Entities.JobOrderItem
            {
                Id = Guid.NewGuid(),
                JobOrderId = jobOrder.Id,
                ServiceId = itemDto.ServiceId,
                ServiceName = itemDto.ServiceName,
                Description = itemDto.Description,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                Subtotal = itemDto.Quantity * itemDto.UnitPrice,
                Notes = itemDto.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            jobOrder.Items.Add(item);
        }

        // Add parts
        foreach (var partDto in dto.Parts)
        {
            var part = new Domain.Entities.JobOrderPart
            {
                Id = Guid.NewGuid(),
                JobOrderId = jobOrder.Id,
                ProductId = partDto.ProductId,
                ProductName = partDto.ProductName,
                ProductSku = partDto.ProductSku,
                Quantity = partDto.Quantity,
                UnitPrice = partDto.UnitPrice,
                Subtotal = partDto.Quantity * partDto.UnitPrice,
                Notes = partDto.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            jobOrder.Parts.Add(part);
        }

        // Calculate totals
        jobOrder.TotalAmount = jobOrder.Items.Sum(i => i.Subtotal) + jobOrder.Parts.Sum(p => p.Subtotal);
        jobOrder.GrandTotal = jobOrder.TotalAmount - jobOrder.DiscountAmount + jobOrder.TaxAmount;

        _context.JobOrders.Add(jobOrder);
        await _context.SaveChangesAsync();

        return jobOrder;
    }

    public async Task<bool> UpdateStatusAsync(Guid id, UpdateJobOrderStatusDto dto)
    {
        var jobOrder = await _context.JobOrders.FindAsync(id);
        if (jobOrder == null || jobOrder.DeletedAt != null)
            return false;

        jobOrder.Status = Enum.Parse<JobOrderStatus>(dto.Status.ToUpper());
        jobOrder.UpdatedAt = DateTime.UtcNow;

        if (jobOrder.Status == JobOrderStatus.IN_PROGRESS && jobOrder.StartTime == null)
        {
            jobOrder.StartTime = DateTime.UtcNow;
        }
        else if (jobOrder.Status == JobOrderStatus.COMPLETED && jobOrder.EndTime == null)
        {
            jobOrder.EndTime = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AssignMechanicsAsync(Guid id, AssignMechanicDto dto)
    {
        var jobOrder = await _context.JobOrders.FindAsync(id);
        if (jobOrder == null || jobOrder.DeletedAt != null)
            return false;

        jobOrder.AssignedMechanicIds = dto.MechanicIds;
        jobOrder.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var jobOrder = await _context.JobOrders.FindAsync(id);
        if (jobOrder == null || jobOrder.DeletedAt != null)
            return false;

        jobOrder.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Domain.Entities.JobOrder>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.JobOrders
            .Include(jo => jo.Items)
            .Include(jo => jo.Parts)
            .Where(jo => jo.CustomerId == customerId && jo.DeletedAt == null)
            .OrderByDescending(jo => jo.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Domain.Entities.JobOrder>> GetByVehicleIdAsync(Guid vehicleId)
    {
        return await _context.JobOrders
            .Include(jo => jo.Items)
            .Include(jo => jo.Parts)
            .Where(jo => jo.VehicleId == vehicleId && jo.DeletedAt == null)
            .OrderByDescending(jo => jo.CreatedAt)
            .ToListAsync();
    }

    private async Task<string> GenerateJobOrderNumberAsync(Guid tenantId)
    {
        var today = DateTime.UtcNow;
        var prefix = $"JO-{today:yyyyMMdd}";
        
        var lastJobOrder = await _context.JobOrders
            .Where(jo => jo.TenantId == tenantId && jo.JobOrderNumber.StartsWith(prefix))
            .OrderByDescending(jo => jo.JobOrderNumber)
            .FirstOrDefaultAsync();

        if (lastJobOrder == null)
        {
            return $"{prefix}-0001";
        }

        var lastNumber = int.Parse(lastJobOrder.JobOrderNumber.Split('-').Last());
        return $"{prefix}-{(lastNumber + 1):D4}";
    }
}