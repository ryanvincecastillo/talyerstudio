using Microsoft.EntityFrameworkCore;
using TalyerStudio.Customer.Application.Interfaces;
using TalyerStudio.Customer.Infrastructure.Data;
using TalyerStudio.Shared.Contracts.Customers;

namespace TalyerStudio.Customer.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerDbContext _context;

    public CustomerRepository(CustomerDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
    }

    public async Task<List<Domain.Entities.Customer>> GetAllAsync(Guid tenantId, string? search = null, string? tag = null, int skip = 0, int take = 50)
    {
        var query = _context.Customers.Where(c => c.TenantId == tenantId && c.DeletedAt == null);

        // Search by name, email, or phone
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c =>
                c.FirstName.Contains(search) ||
                c.LastName.Contains(search) ||
                c.Email.Contains(search) ||
                c.PhoneNumber.Contains(search));
        }

        // Filter by tag
        if (!string.IsNullOrEmpty(tag))
        {
            query = query.Where(c => c.Tags.Contains(tag));
        }

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<Domain.Entities.Customer> CreateAsync(CreateCustomerDto dto, Guid tenantId)
    {
        var customer = new Domain.Entities.Customer
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Street = dto.Street,
            Barangay = dto.Barangay,
            Municipality = dto.Municipality,
            Province = dto.Province,
            ZipCode = dto.ZipCode,
            Birthday = dto.Birthday,
            LoyaltyPoints = 0,
            CustomerType = dto.CustomerType ?? "REGULAR",
            Tags = dto.Tags ?? new string[] { },
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateCustomerDto dto)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (customer == null)
        {
            return false;
        }

        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.Email = dto.Email;
        customer.PhoneNumber = dto.PhoneNumber;
        customer.Street = dto.Street;
        customer.Barangay = dto.Barangay;
        customer.Municipality = dto.Municipality;
        customer.Province = dto.Province;
        customer.ZipCode = dto.ZipCode;
        customer.Birthday = dto.Birthday;
        customer.CustomerType = dto.CustomerType ?? customer.CustomerType;
        customer.Tags = dto.Tags ?? customer.Tags;
        customer.Notes = dto.Notes;
        customer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (customer == null)
        {
            return false;
        }

        customer.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsByEmailAsync(string email, Guid tenantId, Guid? excludeCustomerId = null)
    {
        var query = _context.Customers
            .Where(c => c.TenantId == tenantId && c.Email == email && c.DeletedAt == null);

        if (excludeCustomerId.HasValue)
        {
            query = query.Where(c => c.Id != excludeCustomerId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<bool> ExistsByPhoneAsync(string phoneNumber, Guid tenantId, Guid? excludeCustomerId = null)
    {
        var query = _context.Customers
            .Where(c => c.TenantId == tenantId && c.PhoneNumber == phoneNumber && c.DeletedAt == null);

        if (excludeCustomerId.HasValue)
        {
            query = query.Where(c => c.Id != excludeCustomerId.Value);
        }

        return await query.AnyAsync();
    }
}