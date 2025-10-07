using Microsoft.AspNetCore.Mvc;
using TalyerStudio.Shared.Infrastructure.Grpc;

namespace TalyerStudio.Vehicle.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly CustomerService.CustomerServiceClient _customerClient;
    private readonly ILogger<VehiclesController> _logger;

    public VehiclesController(
        CustomerService.CustomerServiceClient customerClient,
        ILogger<VehiclesController> logger)
    {
        _customerClient = customerClient;
        _logger = logger;
    }

    [HttpGet("test-grpc/{customerId}")]
    public async Task<ActionResult> TestGrpcCall(string customerId)
    {
        try
        {
            _logger.LogInformation("Testing gRPC call to Customer Service for ID: {CustomerId}", customerId);

            var request = new GetCustomerRequest { CustomerId = customerId };
            var response = await _customerClient.GetCustomerAsync(request);

            return Ok(new
            {
                Message = "gRPC call successful!",
                Customer = new
                {
                    response.Id,
                    response.FirstName,
                    response.LastName,
                    response.Email,
                    response.PhoneNumber
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Customer Service via gRPC");
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("test-grpc-list")]
    public async Task<ActionResult> TestGrpcList()
    {
        try
        {
            _logger.LogInformation("Testing gRPC GetCustomers call");

            var request = new GetCustomersRequest { PageSize = 10 };
            var response = await _customerClient.GetCustomersAsync(request);

            return Ok(new
            {
                Message = "gRPC call successful!",
                TotalCount = response.TotalCount,
                Customers = response.Customers.Select(c => new
                {
                    c.Id,
                    c.FirstName,
                    c.LastName,
                    c.Email
                })
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Customer Service via gRPC");
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}
