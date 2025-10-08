namespace TalyerStudio.JobOrder.Application.DTOs;

public class CreateJobOrderDto
{
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public string Priority { get; set; } = "NORMAL";
    
    public int OdometerReading { get; set; }
    public string CustomerComplaints { get; set; } = string.Empty;
    public string? InspectionNotes { get; set; }
    
    public DateTime? EstimatedCompletionTime { get; set; }
    
    public List<CreateJobOrderItemDto> Items { get; set; } = new();
    public List<CreateJobOrderPartDto> Parts { get; set; } = new();
}

public class CreateJobOrderItemDto
{
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public string? Notes { get; set; }
}

public class CreateJobOrderPartDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ProductSku { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Notes { get; set; }
}