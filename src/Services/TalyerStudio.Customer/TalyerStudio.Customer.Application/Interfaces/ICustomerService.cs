using TalyerStudio.Shared.Contracts.Customers;

namespace TalyerStudio.Customer.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto?> GetByIdAsync(Guid id);
    Task<List<CustomerDto>> GetAllAsync(Guid tenantId, string? search = null, string? tag = null, int skip = 0, int take = 50);
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto, Guid tenantId);
    Task<bool> UpdateAsync(Guid id, UpdateCustomerDto dto);
    Task<bool> DeleteAsync(Guid id);
}