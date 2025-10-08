using TalyerStudio.JobOrder.Domain.Enums;

namespace TalyerStudio.JobOrder.Domain.Entities;

public class JobOrder : BaseEntity
{
    public string JobOrderNumber { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public Guid? BranchId { get; set; }
    
    // Customer & Vehicle References
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    
    // Status & Priority
    public JobOrderStatus Status { get; set; } = JobOrderStatus.PENDING;
    public JobOrderPriority Priority { get; set; } = JobOrderPriority.NORMAL;
    
    // Vehicle Information
    public int OdometerReading { get; set; }
    
    // Customer Complaints & Notes
    public string CustomerComplaints { get; set; } = string.Empty;
    public string InspectionNotes { get; set; } = string.Empty;
    
    // Photos
    public string[] BeforePhotos { get; set; } = Array.Empty<string>();
    public string[] AfterPhotos { get; set; } = Array.Empty<string>();
    
    // Mechanic Assignments
    public Guid[] AssignedMechanicIds { get; set; } = Array.Empty<Guid>();
    
    // Timing
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime? EstimatedCompletionTime { get; set; }
    
    // Financial
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal GrandTotal { get; set; }
    
    // Navigation Properties
    public virtual ICollection<JobOrderItem> Items { get; set; } = new List<JobOrderItem>();
    public virtual ICollection<JobOrderPart> Parts { get; set; } = new List<JobOrderPart>();
}