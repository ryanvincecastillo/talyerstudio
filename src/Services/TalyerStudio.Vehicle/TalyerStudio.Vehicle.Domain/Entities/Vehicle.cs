namespace TalyerStudio.Vehicle.Domain.Entities;

public class Vehicle
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid CustomerId { get; set; }
    
    // Basic Info
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;
    public string PlateNumber { get; set; } = string.Empty;
    
    // Identification
    public string? EngineNumber { get; set; }
    public string? ChassisNumber { get; set; }
    
    // Vehicle Type
    public VehicleType VehicleType { get; set; }
    public string? VehicleCategory { get; set; } // For motorcycles: Sport, Commuter, etc.
    
    // Engine Details
    public int? Displacement { get; set; } // in cc (e.g., 110, 125, 1500)
    public string? FuelType { get; set; } // Gasoline, Diesel, Electric
    public string? Transmission { get; set; } // Manual, Automatic, CVT
    
    // Odometer
    public int CurrentOdometer { get; set; } // in kilometers
    
    // OR/CR Tracking (Philippines-specific)
    public DateTime? OrCrExpiryDate { get; set; }
    
    // Motorcycle-specific
    public string? TireSizeFront { get; set; } // e.g., "70/90-17"
    public string? TireSizeRear { get; set; }  // e.g., "80/90-17"
    
    // Images
    public string[] Images { get; set; } = Array.Empty<string>();
    
    // QR Code for quick lookup
    public string? QrCode { get; set; }
    
    // Notes
    public string? Notes { get; set; }
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}

public enum VehicleType
{
    MOTORCYCLE,
    CAR,
    SUV,
    TRUCK,
    VAN,
    PICKUP
}