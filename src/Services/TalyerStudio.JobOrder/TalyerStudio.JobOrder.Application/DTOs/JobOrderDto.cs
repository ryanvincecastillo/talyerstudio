using TalyerStudio.JobOrder.Domain.Enums;
using TalyerStudio.JobOrder.Application.DTOs;

namespace TalyerStudio.JobOrder.Application.DTOs;

public class JobOrderDto
{
    public Guid Id { get; set; }
    public string JobOrderNumber { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public Guid? BranchId { get; set; }
    
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    
    public int OdometerReading { get; set; }
    public string CustomerComplaints { get; set; } = string.Empty;
    public string InspectionNotes { get; set; } = string.Empty;
    
    public string[] BeforePhotos { get; set; } = Array.Empty<string>();
    public string[] AfterPhotos { get; set; } = Array.Empty<string>();
    public Guid[] AssignedMechanicIds { get; set; } = Array.Empty<Guid>();
    
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime? EstimatedCompletionTime { get; set; }
    
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal GrandTotal { get; set; }
    
    public List<JobOrderItemDto> Items { get; set; } = new();
    public List<JobOrderPartDto> Parts { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}