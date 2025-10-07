using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TalyerStudio.Customer.Infrastructure.Data;
using TalyerStudio.Shared.Infrastructure.Grpc;

namespace TalyerStudio.Customer.API.Services;

public class CustomerGrpcService : CustomerService.CustomerServiceBase
{
    private readonly CustomerDbContext _context;
    private readonly ILogger<CustomerGrpcService> _logger;

    public CustomerGrpcService(CustomerDbContext context, ILogger<CustomerGrpcService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override async Task<CustomerResponse> GetCustomer(GetCustomerRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetCustomer called for ID: {CustomerId}", request.CustomerId);

        if (!Guid.TryParse(request.CustomerId, out var customerId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid customer ID format"));
        }

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == customerId && c.DeletedAt == null);

        if (customer == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
        }

        return new CustomerResponse
        {
            Id = customer.Id.ToString(),
            TenantId = customer.TenantId.ToString(),
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Barangay = customer.Barangay ?? string.Empty,
            Municipality = customer.Municipality ?? string.Empty,
            Province = customer.Province ?? string.Empty
        };
    }

    public override async Task<CustomersResponse> GetCustomers(GetCustomersRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetCustomers called for TenantId: {TenantId}", request.TenantId);

        var query = _context.Customers.Where(c => c.DeletedAt == null);

        if (!string.IsNullOrEmpty(request.TenantId) && Guid.TryParse(request.TenantId, out var tenantId))
        {
            query = query.Where(c => c.TenantId == tenantId);
        }

        var totalCount = await query.CountAsync();
        var pageSize = request.PageSize > 0 ? request.PageSize : 50;

        var customers = await query
            .OrderByDescending(c => c.CreatedAt)
            .Take(pageSize)
            .ToListAsync();

        var response = new CustomersResponse
        {
            TotalCount = totalCount
        };

        foreach (var customer in customers)
        {
            response.Customers.Add(new CustomerResponse
            {
                Id = customer.Id.ToString(),
                TenantId = customer.TenantId.ToString(),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Barangay = customer.Barangay ?? string.Empty,
                Municipality = customer.Municipality ?? string.Empty,
                Province = customer.Province ?? string.Empty
            });
        }

        return response;
    }
}
