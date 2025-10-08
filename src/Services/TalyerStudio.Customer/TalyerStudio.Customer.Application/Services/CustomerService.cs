using TalyerStudio.Customer.Application.Interfaces;
using TalyerStudio.Shared.Contracts.Customers;

namespace TalyerStudio.Customer.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        var customer = await _repository.GetByIdAsync(id);
        return customer == null ? null : MapToDto(customer);
    }

    public async Task<List<CustomerDto>> GetAllAsync(Guid tenantId, string? search = null, string? tag = null, int skip = 0, int take = 50)
    {
        var customers = await _repository.GetAllAsync(tenantId, search, tag, skip, take);
        return customers.Select(MapToDto).ToList();
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto, Guid tenantId)
    {
        // Validate email uniqueness
        if (!string.IsNullOrEmpty(dto.Email))
        {
            var emailExists = await _repository.ExistsByEmailAsync(dto.Email, tenantId);
            if (emailExists)
            {
                throw new InvalidOperationException($"Customer with email '{dto.Email}' already exists");
            }
        }

        // Validate phone uniqueness
        var phoneExists = await _repository.ExistsByPhoneAsync(dto.PhoneNumber, tenantId);
        if (phoneExists)
        {
            throw new InvalidOperationException($"Customer with phone number '{dto.PhoneNumber}' already exists");
        }

        var customer = await _repository.CreateAsync(dto, tenantId);
        return MapToDto(customer);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateCustomerDto dto)
    {
        // Get existing customer
        var existingCustomer = await _repository.GetByIdAsync(id);
        if (existingCustomer == null)
        {
            throw new InvalidOperationException("Customer not found");
        }

        // Validate email uniqueness (if changed)
        if (!string.IsNullOrEmpty(dto.Email) && dto.Email != existingCustomer.Email)
        {
            var emailExists = await _repository.ExistsByEmailAsync(dto.Email, existingCustomer.TenantId, id);
            if (emailExists)
            {
                throw new InvalidOperationException($"Customer with email '{dto.Email}' already exists");
            }
        }

        // Validate phone uniqueness (if changed)
        if (dto.PhoneNumber != existingCustomer.PhoneNumber)
        {
            var phoneExists = await _repository.ExistsByPhoneAsync(dto.PhoneNumber, existingCustomer.TenantId, id);
            if (phoneExists)
            {
                throw new InvalidOperationException($"Customer with phone number '{dto.PhoneNumber}' already exists");
            }
        }

        return await _repository.UpdateAsync(id, dto);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private CustomerDto MapToDto(Domain.Entities.Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            TenantId = customer.TenantId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Street = customer.Street,
            Barangay = customer.Barangay,
            Municipality = customer.Municipality,
            Province = customer.Province,
            ZipCode = customer.ZipCode,
            Birthday = customer.Birthday,
            LoyaltyPoints = customer.LoyaltyPoints,
            CustomerType = customer.CustomerType,
            Tags = customer.Tags,
            Notes = customer.Notes,
            CreatedAt = customer.CreatedAt
        };
    }
}