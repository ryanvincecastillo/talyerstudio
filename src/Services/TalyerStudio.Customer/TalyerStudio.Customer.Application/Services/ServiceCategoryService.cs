using TalyerStudio.Customer.Application.Interfaces;
using TalyerStudio.Customer.Domain.Entities;
using TalyerStudio.Shared.Contracts.Services;

namespace TalyerStudio.Customer.Application.Services;

public class ServiceCategoryService : IServiceCategoryService
{
    private readonly IServiceCategoryRepository _repository;

    public ServiceCategoryService(IServiceCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceCategoryDto?> GetByIdAsync(Guid id)
    {
        var category = await _repository.GetByIdAsync(id);
        return category == null ? null : MapToDto(category);
    }

    public async Task<List<ServiceCategoryDto>> GetAllAsync(Guid tenantId, bool? isActive = null)
    {
        var categories = await _repository.GetAllAsync(tenantId, isActive);
        return categories.Select(MapToDto).ToList();
    }

    public async Task<ServiceCategoryDto> CreateAsync(CreateServiceCategoryDto dto, Guid tenantId)
    {
        // Validate name uniqueness
        var nameExists = await _repository.ExistsByNameAsync(dto.Name, tenantId);
        if (nameExists)
        {
            throw new InvalidOperationException($"Service category with name '{dto.Name}' already exists");
        }

        var category = await _repository.CreateAsync(dto, tenantId);
        return MapToDto(category);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateServiceCategoryDto dto)
    {
        // Get existing category
        var existingCategory = await _repository.GetByIdAsync(id);
        if (existingCategory == null)
        {
            throw new InvalidOperationException("Service category not found");
        }

        // Validate name uniqueness (if changed)
        if (dto.Name != existingCategory.Name)
        {
            var nameExists = await _repository.ExistsByNameAsync(dto.Name, existingCategory.TenantId, id);
            if (nameExists)
            {
                throw new InvalidOperationException($"Service category with name '{dto.Name}' already exists");
            }
        }

        return await _repository.UpdateAsync(id, dto);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private ServiceCategoryDto MapToDto(ServiceCategory category)
    {
        return new ServiceCategoryDto
        {
            Id = category.Id,
            TenantId = category.TenantId,
            Name = category.Name,
            Description = category.Description,
            Icon = category.Icon,
            IsActive = category.IsActive,
            DisplayOrder = category.DisplayOrder,
            CreatedAt = category.CreatedAt
        };
    }
}