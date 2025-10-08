namespace TalyerStudio.Shared.Contracts.Customers;

public class CustomerDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Street { get; set; }
    public string? Barangay { get; set; }
    public string? Municipality { get; set; }
    public string? Province { get; set; }
    public string? ZipCode { get; set; }
    public DateTime? Birthday { get; set; }
    public int LoyaltyPoints { get; set; }
    public string CustomerType { get; set; } = string.Empty;
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateCustomerDto
{
    public Guid TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Street { get; set; }
    public string? Barangay { get; set; }
    public string? Municipality { get; set; }
    public string? Province { get; set; }
    public string? ZipCode { get; set; }
    public DateTime? Birthday { get; set; }
    public string CustomerType { get; set; } = "INDIVIDUAL";
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string? Notes { get; set; }
}

public class UpdateCustomerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Street { get; set; }
    public string? Barangay { get; set; }
    public string? Municipality { get; set; }
    public string? Province { get; set; }
    public string? ZipCode { get; set; }
    public DateTime? Birthday { get; set; }
    public string CustomerType { get; set; } = "INDIVIDUAL";
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string? Notes { get; set; }
}