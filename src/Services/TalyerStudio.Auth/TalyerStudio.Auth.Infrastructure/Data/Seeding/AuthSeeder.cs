using Microsoft.EntityFrameworkCore;
using TalyerStudio.Auth.Domain.Entities;
using TalyerStudio.Auth.Infrastructure.Data;

namespace TalyerStudio.Auth.Infrastructure.Data.Seeding;

public static class AuthSeeder
{
    public static async Task SeedAsync(AuthDbContext context)
    {
        // Seed Permissions
        if (!context.Permissions.Any())
        {
            var permissions = new List<Permission>
            {
                // Customer permissions
                new Permission { Id = Guid.NewGuid(), Name = "customers.view", Description = "View customers", Module = "customers", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "customers.create", Description = "Create customers", Module = "customers", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "customers.edit", Description = "Edit customers", Module = "customers", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "customers.delete", Description = "Delete customers", Module = "customers", CreatedAt = DateTime.UtcNow },
                
                // Vehicle permissions
                new Permission { Id = Guid.NewGuid(), Name = "vehicles.view", Description = "View vehicles", Module = "vehicles", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "vehicles.create", Description = "Create vehicles", Module = "vehicles", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "vehicles.edit", Description = "Edit vehicles", Module = "vehicles", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "vehicles.delete", Description = "Delete vehicles", Module = "vehicles", CreatedAt = DateTime.UtcNow },
                
                // Job Order permissions
                new Permission { Id = Guid.NewGuid(), Name = "job_orders.view", Description = "View job orders", Module = "job_orders", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "job_orders.create", Description = "Create job orders", Module = "job_orders", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "job_orders.edit", Description = "Edit job orders", Module = "job_orders", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "job_orders.delete", Description = "Delete job orders", Module = "job_orders", CreatedAt = DateTime.UtcNow },
                
                // Inventory permissions
                new Permission { Id = Guid.NewGuid(), Name = "inventory.view", Description = "View inventory", Module = "inventory", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "inventory.create", Description = "Create inventory items", Module = "inventory", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "inventory.edit", Description = "Edit inventory items", Module = "inventory", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "inventory.delete", Description = "Delete inventory items", Module = "inventory", CreatedAt = DateTime.UtcNow },
                
                // Invoice permissions
                new Permission { Id = Guid.NewGuid(), Name = "invoices.view", Description = "View invoices", Module = "invoices", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "invoices.create", Description = "Create invoices", Module = "invoices", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "invoices.edit", Description = "Edit invoices", Module = "invoices", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "invoices.delete", Description = "Delete invoices", Module = "invoices", CreatedAt = DateTime.UtcNow },
                
                // User management permissions
                new Permission { Id = Guid.NewGuid(), Name = "users.view", Description = "View users", Module = "users", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "users.create", Description = "Create users", Module = "users", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "users.edit", Description = "Edit users", Module = "users", CreatedAt = DateTime.UtcNow },
                new Permission { Id = Guid.NewGuid(), Name = "users.delete", Description = "Delete users", Module = "users", CreatedAt = DateTime.UtcNow },
            };

            await context.Permissions.AddRangeAsync(permissions);
            await context.SaveChangesAsync();
        }

        // Seed default tenant for testing
        var defaultTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        // Seed Roles for default tenant
        if (!context.Roles.Any(r => r.TenantId == defaultTenantId))
        {
            var adminRole = new Role
            {
                Id = Guid.NewGuid(),
                TenantId = defaultTenantId,
                Name = "Admin",
                Description = "Administrator with full access",
                IsSystemRole = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var userRole = new Role
            {
                Id = Guid.NewGuid(),
                TenantId = defaultTenantId,
                Name = "User",
                Description = "Regular user with limited access",
                IsSystemRole = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await context.Roles.AddRangeAsync(adminRole, userRole);
            await context.SaveChangesAsync();

            // Assign all permissions to Admin role
            var allPermissions = await context.Permissions.ToListAsync();
            var adminRolePermissions = allPermissions.Select(p => new RolePermission
            {
                RoleId = adminRole.Id,
                PermissionId = p.Id,
                AssignedAt = DateTime.UtcNow
            }).ToList();

            await context.RolePermissions.AddRangeAsync(adminRolePermissions);
            await context.SaveChangesAsync();

            // Assign basic permissions to User role (view only)
            var viewPermissions = allPermissions.Where(p => p.Name.EndsWith(".view")).ToList();
            var userRolePermissions = viewPermissions.Select(p => new RolePermission
            {
                RoleId = userRole.Id,
                PermissionId = p.Id,
                AssignedAt = DateTime.UtcNow
            }).ToList();

            await context.RolePermissions.AddRangeAsync(userRolePermissions);
            await context.SaveChangesAsync();
        }
    }
}