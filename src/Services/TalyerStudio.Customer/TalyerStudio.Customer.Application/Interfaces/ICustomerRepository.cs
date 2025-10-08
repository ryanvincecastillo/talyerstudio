using TalyerStudio.Customer.Domain.Entities;
using TalyerStudio.Shared.Contracts.Customers;

namespace TalyerStudio.Customer.Application.Interfaces;

public interface ICustomerRepository
{
    Task<Domain.Entities.Customer?> GetByIdAsync(Guid id);
    Task<List<Domain.Entities.Customer>> GetAllAsync(Guid tenantId, string? search = null, string? tag = null, int skip = 0, int take = 50);
    Task<Domain.Entities.Customer> CreateAsync(CreateCustomerDto dto, Guid tenantId);
    Task<bool> UpdateAsync(Guid id, UpdateCustomerDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsByEmailAsync(string email, Guid tenantId, Guid? excludeCustomerId = null);
    Task<bool> ExistsByPhoneAsync(string phoneNumber, Guid tenantId, Guid? excludeCustomerId = null);
}