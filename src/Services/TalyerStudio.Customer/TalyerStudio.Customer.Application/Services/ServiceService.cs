using TalyerStudio.Customer.Application.Interfaces;
using TalyerStudio.Customer.Domain.Entities;
using TalyerStudio.Shared.Contracts.Services;

namespace TalyerStudio.Customer.Application.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _repository;

    public ServiceService(IServiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceDto?> GetByIdAsync(Guid id)
    {
        var service = await _repository.GetByIdAsync(id);
        return service == null ? null : MapToDto(service);
    }

    public async Task<List<ServiceDto>> GetAllAsync(Guid tenantId, Guid? categoryId = null, string? applicability = null, bool? isActive = null, int skip = 0, int take = 50)
    {
        var services = await _repository.GetAllAsync(tenantId, categoryId, applicability, isActive, skip, take);
        return services.Select(MapToDto).ToList();
    }

    public async Task<ServiceDto> CreateAsync(CreateServiceDto dto, Guid tenantId)
    {
        var service = await _repository.CreateAsync(dto, tenantId);
        return MapToDto(service);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateServiceDto dto)
    {
        var existingService = await _repository.GetByIdAsync(id);
        if (existingService == null)
        {
            throw new InvalidOperationException("Service not found");
        }

        return await _repository.UpdateAsync(id, dto);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<List<ServiceDto>> GetByCategoryIdAsync(Guid categoryId)
    {
        var services = await _repository.GetByCategoryIdAsync(categoryId);
        return services.Select(MapToDto).ToList();
    }

    private ServiceDto MapToDto(Service service)
    {
        return new ServiceDto
        {
            Id = service.Id,
            TenantId = service.TenantId,
            Name = service.Name,
            Description = service.Description,
            CategoryId = service.CategoryId,
            CategoryName = service.Category?.Name,
            BasePrice = service.BasePrice,
            Currency = service.Currency,
            Applicability = service.Applicability.ToString(),
            EstimatedDurationMinutes = service.EstimatedDurationMinutes,
            IsActive = service.IsActive,
            DisplayOrder = service.DisplayOrder,
            Icon = service.Icon,
            CreatedAt = service.CreatedAt
        };
    }
}