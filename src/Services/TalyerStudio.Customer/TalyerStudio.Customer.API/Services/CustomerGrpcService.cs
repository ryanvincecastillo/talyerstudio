using Grpc.Core;
using TalyerStudio.Customer.Application.Interfaces;
using TalyerStudio.Shared.Protos.Customer;

namespace TalyerStudio.Customer.API.Services;

public class CustomerGrpcService : Shared.Protos.Customer.CustomerService.CustomerServiceBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomerGrpcService> _logger;

    public CustomerGrpcService(ICustomerService customerService, ILogger<CustomerGrpcService> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    public override async Task<CustomerResponse> GetCustomer(GetCustomerRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetCustomer called for {CustomerId}", request.CustomerId);

        if (!Guid.TryParse(request.CustomerId, out var customerId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid customer ID format"));
        }

        var customer = await _customerService.GetByIdAsync(customerId);

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
            Email = customer.Email ?? "",
            PhoneNumber = customer.PhoneNumber,
            Street = customer.Street ?? "",
            Barangay = customer.Barangay ?? "",
            Municipality = customer.Municipality ?? "",
            Province = customer.Province ?? "",
            ZipCode = customer.ZipCode ?? "",
            LoyaltyPoints = customer.LoyaltyPoints,
            CustomerType = customer.CustomerType ?? "REGULAR"
        };
    }

    public override async Task<CustomersResponse> GetCustomers(GetCustomersRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetCustomers called for tenant {TenantId}", request.TenantId);

        if (!Guid.TryParse(request.TenantId, out var tenantId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid tenant ID format"));
        }

        var customers = await _customerService.GetAllAsync(
            tenantId,
            string.IsNullOrEmpty(request.Search) ? null : request.Search,
            string.IsNullOrEmpty(request.Tag) ? null : request.Tag,
            request.Skip,
            request.Take > 0 ? request.Take : 50
        );

        var response = new CustomersResponse
        {
            TotalCount = customers.Count
        };

        foreach (var customer in customers)
        {
            response.Customers.Add(new CustomerResponse
            {
                Id = customer.Id.ToString(),
                TenantId = customer.TenantId.ToString(),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email ?? "",
                PhoneNumber = customer.PhoneNumber,
                Street = customer.Street ?? "",
                Barangay = customer.Barangay ?? "",
                Municipality = customer.Municipality ?? "",
                Province = customer.Province ?? "",
                ZipCode = customer.ZipCode ?? "",
                LoyaltyPoints = customer.LoyaltyPoints,
                CustomerType = customer.CustomerType ?? "REGULAR"
            });
        }

        return response;
    }

    public override async Task<CustomersResponse> GetCustomersByIds(GetCustomersByIdsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetCustomersByIds called with {Count} IDs", request.CustomerIds.Count);

        if (!Guid.TryParse(request.TenantId, out var tenantId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid tenant ID format"));
        }

        var response = new CustomersResponse();

        foreach (var customerIdStr in request.CustomerIds)
        {
            if (Guid.TryParse(customerIdStr, out var customerId))
            {
                var customer = await _customerService.GetByIdAsync(customerId);
                if (customer != null && customer.TenantId == tenantId)
                {
                    response.Customers.Add(new CustomerResponse
                    {
                        Id = customer.Id.ToString(),
                        TenantId = customer.TenantId.ToString(),
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email ?? "",
                        PhoneNumber = customer.PhoneNumber,
                        Street = customer.Street ?? "",
                        Barangay = customer.Barangay ?? "",
                        Municipality = customer.Municipality ?? "",
                        Province = customer.Province ?? "",
                        ZipCode = customer.ZipCode ?? "",
                        LoyaltyPoints = customer.LoyaltyPoints,
                        CustomerType = customer.CustomerType ?? "REGULAR"
                    });
                }
            }
        }

        response.TotalCount = response.Customers.Count;
        return response;
    }

    public override async Task<CustomerExistsResponse> CustomerExists(CustomerExistsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC CustomerExists called for {CustomerId}", request.CustomerId);

        if (!Guid.TryParse(request.CustomerId, out var customerId))
        {
            return new CustomerExistsResponse { Exists = false };
        }

        var customer = await _customerService.GetByIdAsync(customerId);
        return new CustomerExistsResponse { Exists = customer != null };
    }
}