namespace TalyerStudio.Shared.Contracts.Vehicles;

public class VehicleDto
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
    public string VehicleType { get; set; } = string.Empty;
    public string? VehicleCategory { get; set; }
    
    // Engine Details
    public int? Displacement { get; set; }
    public string? FuelType { get; set; }
    public string? Transmission { get; set; }
    
    // Odometer
    public int CurrentOdometer { get; set; }
    
    // OR/CR (Philippines)
    public DateTime? OrCrExpiryDate { get; set; }
    
    // Motorcycle-specific
    public string? TireSizeFront { get; set; }
    public string? TireSizeRear { get; set; }
    
    // Images
    public string[] Images { get; set; } = Array.Empty<string>();
    
    // QR Code
    public string? QrCode { get; set; }
    
    // Notes
    public string? Notes { get; set; }
    
    // Audit
    public DateTime CreatedAt { get; set; }
    
    // Computed Properties
    public string DisplayName => $"{Year} {Make} {Model}";
    public bool IsMotorcycle => VehicleType == "MOTORCYCLE";
}

public class CreateVehicleDto
{
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
    public string VehicleType { get; set; } = "CAR"; // Default to CAR
    public string? VehicleCategory { get; set; }
    
    // Engine Details
    public int? Displacement { get; set; }
    public string? FuelType { get; set; }
    public string? Transmission { get; set; }
    
    // Odometer
    public int CurrentOdometer { get; set; } = 0;
    
    // OR/CR (Philippines)
    public DateTime? OrCrExpiryDate { get; set; }
    
    // Motorcycle-specific
    public string? TireSizeFront { get; set; }
    public string? TireSizeRear { get; set; }
    
    // Images
    public string[] Images { get; set; } = Array.Empty<string>();
    
    // Notes
    public string? Notes { get; set; }
}

public class UpdateVehicleDto
{
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
    public string VehicleType { get; set; } = string.Empty;
    public string? VehicleCategory { get; set; }
    
    // Engine Details
    public int? Displacement { get; set; }
    public string? FuelType { get; set; }
    public string? Transmission { get; set; }
    
    // Odometer
    public int CurrentOdometer { get; set; }
    
    // OR/CR (Philippines)
    public DateTime? OrCrExpiryDate { get; set; }
    
    // Motorcycle-specific
    public string? TireSizeFront { get; set; }
    public string? TireSizeRear { get; set; }
    
    // Images
    public string[] Images { get; set; } = Array.Empty<string>();
    
    // Notes
    public string? Notes { get; set; }
}