using Grpc.Core;
using TalyerStudio.Vehicle.Application.Interfaces;
using TalyerStudio.Shared.Protos.Vehicle;

namespace TalyerStudio.Vehicle.API.Services;

public class VehicleGrpcService : Shared.Protos.Vehicle.VehicleService.VehicleServiceBase
{
    private readonly IVehicleService _vehicleService;
    private readonly ILogger<VehicleGrpcService> _logger;

    public VehicleGrpcService(IVehicleService vehicleService, ILogger<VehicleGrpcService> logger)
    {
        _vehicleService = vehicleService;
        _logger = logger;
    }

    public override async Task<VehicleResponse> GetVehicle(GetVehicleRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetVehicle called for {VehicleId}", request.VehicleId);

        if (!Guid.TryParse(request.VehicleId, out var vehicleId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid vehicle ID format"));
        }

        var vehicle = await _vehicleService.GetByIdAsync(vehicleId);

        if (vehicle == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Vehicle not found"));
        }

        return new VehicleResponse
        {
            Id = vehicle.Id.ToString(),
            TenantId = vehicle.TenantId.ToString(),
            CustomerId = vehicle.CustomerId.ToString(),
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year,
            Color = vehicle.Color ?? string.Empty,
            PlateNumber = vehicle.PlateNumber,
            EngineNumber = vehicle.EngineNumber ?? string.Empty,
            ChassisNumber = vehicle.ChassisNumber ?? string.Empty,
            VehicleType = vehicle.VehicleType,
            VehicleCategory = vehicle.VehicleCategory ?? string.Empty,
            Displacement = vehicle.Displacement.HasValue ? vehicle.Displacement.Value.ToString() : string.Empty,
            FuelType = vehicle.FuelType ?? string.Empty,
            Transmission = vehicle.Transmission ?? string.Empty,
            CurrentOdometer = vehicle.CurrentOdometer
        };
    }

    public override async Task<VehiclesResponse> GetVehicles(GetVehiclesRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetVehicles called for tenant {TenantId}", request.TenantId);

        if (!Guid.TryParse(request.TenantId, out var tenantId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid tenant ID format"));
        }

        Guid? customerId = null;
        if (!string.IsNullOrEmpty(request.CustomerId) && Guid.TryParse(request.CustomerId, out var cId))
        {
            customerId = cId;
        }

        var vehicles = await _vehicleService.GetAllAsync(
            tenantId,
            customerId,
            string.IsNullOrEmpty(request.Search) ? null : request.Search,
            request.Skip,
            request.Take > 0 ? request.Take : 50
        );

        var response = new VehiclesResponse
        {
            TotalCount = vehicles.Count
        };

        foreach (var vehicle in vehicles)
        {
            response.Vehicles.Add(new VehicleResponse
            {
                Id = vehicle.Id.ToString(),
                TenantId = vehicle.TenantId.ToString(),
                CustomerId = vehicle.CustomerId.ToString(),
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Color = vehicle.Color ?? string.Empty,
                PlateNumber = vehicle.PlateNumber,
                EngineNumber = vehicle.EngineNumber ?? string.Empty,
                ChassisNumber = vehicle.ChassisNumber ?? string.Empty,
                VehicleType = vehicle.VehicleType,
                VehicleCategory = vehicle.VehicleCategory ?? string.Empty,
                Displacement = vehicle.Displacement.HasValue ? vehicle.Displacement.Value.ToString() : string.Empty,
                FuelType = vehicle.FuelType ?? string.Empty,
                Transmission = vehicle.Transmission ?? string.Empty,
                CurrentOdometer = vehicle.CurrentOdometer
            });
        }

        return response;
    }

    public override async Task<VehiclesResponse> GetVehiclesByCustomer(GetVehiclesByCustomerRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetVehiclesByCustomer called for customer {CustomerId}", request.CustomerId);

        if (!Guid.TryParse(request.CustomerId, out var customerId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid customer ID format"));
        }

        var vehicles = await _vehicleService.GetByCustomerIdAsync(customerId);

        var response = new VehiclesResponse
        {
            TotalCount = vehicles.Count
        };

        foreach (var vehicle in vehicles)
        {
            response.Vehicles.Add(new VehicleResponse
            {
                Id = vehicle.Id.ToString(),
                TenantId = vehicle.TenantId.ToString(),
                CustomerId = vehicle.CustomerId.ToString(),
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Color = vehicle.Color ?? string.Empty,
                PlateNumber = vehicle.PlateNumber,
                EngineNumber = vehicle.EngineNumber ?? string.Empty,
                ChassisNumber = vehicle.ChassisNumber ?? string.Empty,
                VehicleType = vehicle.VehicleType,
                VehicleCategory = vehicle.VehicleCategory ?? string.Empty,
                Displacement = vehicle.Displacement.HasValue ? vehicle.Displacement.Value.ToString() : string.Empty,
                FuelType = vehicle.FuelType ?? string.Empty,
                Transmission = vehicle.Transmission ?? string.Empty,
                CurrentOdometer = vehicle.CurrentOdometer
            });
        }

        return response;
    }

    public override async Task<VehicleExistsResponse> VehicleExists(VehicleExistsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC VehicleExists called for {VehicleId}", request.VehicleId);

        if (!Guid.TryParse(request.VehicleId, out var vehicleId))
        {
            return new VehicleExistsResponse { Exists = false };
        }

        var vehicle = await _vehicleService.GetByIdAsync(vehicleId);
        return new VehicleExistsResponse { Exists = vehicle != null };
    }
}