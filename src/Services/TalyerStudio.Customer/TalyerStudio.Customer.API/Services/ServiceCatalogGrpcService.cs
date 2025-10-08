using Grpc.Core;
using TalyerStudio.Customer.Application.Interfaces;
using TalyerStudio.Shared.Protos.Service;

namespace TalyerStudio.Customer.API.Services;

public class ServiceCatalogGrpcService : Shared.Protos.Service.ServiceCatalogService.ServiceCatalogServiceBase
{
    private readonly IServiceService _serviceService;
    private readonly ILogger<ServiceCatalogGrpcService> _logger;

    public ServiceCatalogGrpcService(IServiceService serviceService, ILogger<ServiceCatalogGrpcService> logger)
    {
        _serviceService = serviceService;
        _logger = logger;
    }

    public override async Task<ServiceResponse> GetService(GetServiceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetService called for {ServiceId}", request.ServiceId);

        if (!Guid.TryParse(request.ServiceId, out var serviceId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid service ID format"));
        }

        var service = await _serviceService.GetByIdAsync(serviceId);

        if (service == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Service not found"));
        }

        return new ServiceResponse
        {
            Id = service.Id.ToString(),
            TenantId = service.TenantId.ToString(),
            Name = service.Name,
            Description = service.Description ?? string.Empty,
            CategoryId = service.CategoryId.ToString(),
            CategoryName = service.CategoryName ?? string.Empty,
            BasePrice = (double)service.BasePrice,
            Currency = service.Currency,
            Applicability = service.Applicability,
            EstimatedDurationMinutes = service.EstimatedDurationMinutes.GetValueOrDefault(0),
            IsActive = service.IsActive
        };
    }

    public override async Task<ServicesResponse> GetServices(GetServicesRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetServices called for tenant {TenantId}", request.TenantId);

        if (!Guid.TryParse(request.TenantId, out var tenantId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid tenant ID format"));
        }

        Guid? categoryId = null;
        if (!string.IsNullOrEmpty(request.CategoryId) && Guid.TryParse(request.CategoryId, out var cId))
        {
            categoryId = cId;
        }

        var services = await _serviceService.GetAllAsync(
            tenantId,
            categoryId,
            string.IsNullOrEmpty(request.Applicability) ? null : request.Applicability,
            request.IsActive ? true : null,
            request.Skip,
            request.Take > 0 ? request.Take : 50
        );

        var response = new ServicesResponse
        {
            TotalCount = services.Count
        };

        foreach (var service in services)
        {
            response.Services.Add(new ServiceResponse
            {
                Id = service.Id.ToString(),
                TenantId = service.TenantId.ToString(),
                Name = service.Name,
                Description = service.Description ?? string.Empty,
                CategoryId = service.CategoryId.ToString(),
                CategoryName = service.CategoryName ?? string.Empty,
                BasePrice = (double)service.BasePrice,
                Currency = service.Currency,
                Applicability = service.Applicability,
                EstimatedDurationMinutes = service.EstimatedDurationMinutes.GetValueOrDefault(0),
                IsActive = service.IsActive
            });
        }

        return response;
    }

    public override async Task<ServicesResponse> GetServicesByCategory(GetServicesByCategoryRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetServicesByCategory called for category {CategoryId}", request.CategoryId);

        if (!Guid.TryParse(request.CategoryId, out var categoryId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid category ID format"));
        }

        var services = await _serviceService.GetByCategoryIdAsync(categoryId);

        var response = new ServicesResponse
        {
            TotalCount = services.Count
        };

        foreach (var service in services)
        {
            response.Services.Add(new ServiceResponse
            {
                Id = service.Id.ToString(),
                TenantId = service.TenantId.ToString(),
                Name = service.Name,
                Description = service.Description ?? string.Empty,
                CategoryId = service.CategoryId.ToString(),
                CategoryName = service.CategoryName ?? string.Empty,
                BasePrice = (double)service.BasePrice,
                Currency = service.Currency,
                Applicability = service.Applicability,
                EstimatedDurationMinutes = service.EstimatedDurationMinutes.GetValueOrDefault(0),
                IsActive = service.IsActive
            });
        }

        return response;
    }
}