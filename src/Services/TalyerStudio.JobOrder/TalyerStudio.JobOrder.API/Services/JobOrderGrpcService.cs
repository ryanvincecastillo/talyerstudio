using Grpc.Core;
using TalyerStudio.JobOrder.Application.Interfaces;
using TalyerStudio.Shared.Protos.JobOrder;

namespace TalyerStudio.JobOrder.API.Services;

public class JobOrderGrpcService : Shared.Protos.JobOrder.JobOrderService.JobOrderServiceBase
{
    private readonly IJobOrderService _jobOrderService;
    private readonly ILogger<JobOrderGrpcService> _logger;

    public JobOrderGrpcService(IJobOrderService jobOrderService, ILogger<JobOrderGrpcService> logger)
    {
        _jobOrderService = jobOrderService;
        _logger = logger;
    }

    public override async Task<JobOrderResponse> GetJobOrder(GetJobOrderRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetJobOrder called for {JobOrderId}", request.JobOrderId);

        if (!Guid.TryParse(request.JobOrderId, out var jobOrderId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid job order ID format"));
        }

        var jobOrder = await _jobOrderService.GetByIdAsync(jobOrderId);

        if (jobOrder == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Job order not found"));
        }

        return new JobOrderResponse
        {
            Id = jobOrder.Id.ToString(),
            JobOrderNumber = jobOrder.JobOrderNumber,
            TenantId = jobOrder.TenantId.ToString(),
            CustomerId = jobOrder.CustomerId.ToString(),
            VehicleId = jobOrder.VehicleId.ToString(),
            Status = jobOrder.Status,
            Priority = jobOrder.Priority ?? "NORMAL",
            OdometerReading = jobOrder.OdometerReading,
            CustomerComplaints = jobOrder.CustomerComplaints ?? "",
            TotalAmount = (double)jobOrder.TotalAmount,
            GrandTotal = (double)jobOrder.GrandTotal
        };
    }

    public override async Task<JobOrdersResponse> GetJobOrders(GetJobOrdersRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetJobOrders called for tenant {TenantId}", request.TenantId);

        if (!Guid.TryParse(request.TenantId, out var tenantId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid tenant ID format"));
        }

        var jobOrders = await _jobOrderService.GetAllAsync(
            tenantId,
            string.IsNullOrEmpty(request.Status) ? null : request.Status,
            request.Skip,
            request.Take > 0 ? request.Take : 50
        );

        var response = new JobOrdersResponse
        {
            TotalCount = jobOrders.Count
        };

        foreach (var jobOrder in jobOrders)
        {
            response.JobOrders.Add(new JobOrderResponse
            {
                Id = jobOrder.Id.ToString(),
                JobOrderNumber = jobOrder.JobOrderNumber,
                TenantId = jobOrder.TenantId.ToString(),
                CustomerId = jobOrder.CustomerId.ToString(),
                VehicleId = jobOrder.VehicleId.ToString(),
                Status = jobOrder.Status,
                Priority = jobOrder.Priority ?? "NORMAL",
                OdometerReading = jobOrder.OdometerReading,
                CustomerComplaints = jobOrder.CustomerComplaints ?? "",
                TotalAmount = (double)jobOrder.TotalAmount,
                GrandTotal = (double)jobOrder.GrandTotal
            });
        }

        return response;
    }

    public override async Task<JobOrdersResponse> GetJobOrdersByCustomer(GetJobOrdersByCustomerRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetJobOrdersByCustomer called for customer {CustomerId}", request.CustomerId);

        if (!Guid.TryParse(request.CustomerId, out var customerId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid customer ID format"));
        }

        var jobOrders = await _jobOrderService.GetByCustomerIdAsync(customerId);

        var response = new JobOrdersResponse
        {
            TotalCount = jobOrders.Count
        };

        foreach (var jobOrder in jobOrders)
        {
            response.JobOrders.Add(new JobOrderResponse
            {
                Id = jobOrder.Id.ToString(),
                JobOrderNumber = jobOrder.JobOrderNumber,
                TenantId = jobOrder.TenantId.ToString(),
                CustomerId = jobOrder.CustomerId.ToString(),
                VehicleId = jobOrder.VehicleId.ToString(),
                Status = jobOrder.Status,
                Priority = jobOrder.Priority ?? "NORMAL",
                OdometerReading = jobOrder.OdometerReading,
                CustomerComplaints = jobOrder.CustomerComplaints ?? "",
                TotalAmount = (double)jobOrder.TotalAmount,
                GrandTotal = (double)jobOrder.GrandTotal
            });
        }

        return response;
    }

    public override async Task<JobOrdersResponse> GetJobOrdersByVehicle(GetJobOrdersByVehicleRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetJobOrdersByVehicle called for vehicle {VehicleId}", request.VehicleId);

        if (!Guid.TryParse(request.VehicleId, out var vehicleId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid vehicle ID format"));
        }

        var jobOrders = await _jobOrderService.GetByVehicleIdAsync(vehicleId);

        var response = new JobOrdersResponse
        {
            TotalCount = jobOrders.Count
        };

        foreach (var jobOrder in jobOrders)
        {
            response.JobOrders.Add(new JobOrderResponse
            {
                Id = jobOrder.Id.ToString(),
                JobOrderNumber = jobOrder.JobOrderNumber,
                TenantId = jobOrder.TenantId.ToString(),
                CustomerId = jobOrder.CustomerId.ToString(),
                VehicleId = jobOrder.VehicleId.ToString(),
                Status = jobOrder.Status,
                Priority = jobOrder.Priority ?? "NORMAL",
                OdometerReading = jobOrder.OdometerReading,
                CustomerComplaints = jobOrder.CustomerComplaints ?? "",
                TotalAmount = (double)jobOrder.TotalAmount,
                GrandTotal = (double)jobOrder.GrandTotal
            });
        }

        return response;
    }
}