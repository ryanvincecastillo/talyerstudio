using Grpc.Core;
using TalyerStudio.Inventory.Application.Interfaces;
using TalyerStudio.Shared.Protos.Inventory;

namespace TalyerStudio.Inventory.API.Services;

public class InventoryGrpcService : Shared.Protos.Inventory.InventoryService.InventoryServiceBase
{
    private readonly IProductService _productService;
    private readonly IStockRepository _stockRepository;
    private readonly ILogger<InventoryGrpcService> _logger;

    public InventoryGrpcService(
        IProductService productService, 
        IStockRepository stockRepository,
        ILogger<InventoryGrpcService> logger)
    {
        _productService = productService;
        _stockRepository = stockRepository;
        _logger = logger;
    }

    public override async Task<ProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetProduct called for {ProductId}", request.ProductId);

        if (!Guid.TryParse(request.ProductId, out var productId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid product ID format"));
        }

        var product = await _productService.GetByIdAsync(productId);

        if (product == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
        }

        return new ProductResponse
        {
            Id = product.Id.ToString(),
            TenantId = product.TenantId.ToString(),
            Sku = product.Sku,
            Name = product.Name,
            Description = product.Description ?? "",
            CategoryId = product.CategoryId.ToString(),
            CategoryName = product.CategoryName ?? "",
            ProductType = product.ProductType ?? "PART",
            UnitPrice = (double)product.UnitPrice,
            CostPrice = product.CostPrice.HasValue ? (double)product.CostPrice.Value : 0,
            Currency = product.Currency ?? "PHP",
            UnitOfMeasure = product.Unit ?? "pcs",
            ReorderLevel = product.ReorderLevel,
            ReorderQuantity = product.MaxStockLevel ?? 0,
            IsActive = product.IsActive,
            Barcode = product.Barcode ?? "",
            Brand = product.Brand ?? "",
            Model = product.Model ?? ""
        };
    }

    public override async Task<ProductsResponse> GetProducts(GetProductsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetProducts called for tenant {TenantId}", request.TenantId);

        if (!Guid.TryParse(request.TenantId, out var tenantId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid tenant ID format"));
        }

        Guid? categoryId = null;
        if (!string.IsNullOrEmpty(request.CategoryId) && Guid.TryParse(request.CategoryId, out var catId))
        {
            categoryId = catId;
        }

        var products = await _productService.GetAllAsync(
            tenantId,
            categoryId,
            string.IsNullOrEmpty(request.Search) ? null : request.Search,
            request.IsActive ? true : null,
            request.Skip,
            request.Take > 0 ? request.Take : 50
        );

        var response = new ProductsResponse
        {
            TotalCount = products.Count
        };

        foreach (var product in products)
        {
            response.Products.Add(new ProductResponse
            {
                Id = product.Id.ToString(),
                TenantId = product.TenantId.ToString(),
                Sku = product.Sku,
                Name = product.Name,
                Description = product.Description ?? "",
                CategoryId = product.CategoryId.ToString(),
                CategoryName = product.CategoryName ?? "",
                ProductType = product.ProductType ?? "PART",
                UnitPrice = (double)product.UnitPrice,
                CostPrice = product.CostPrice.HasValue ? (double)product.CostPrice.Value : 0,
                Currency = product.Currency ?? "PHP",
                UnitOfMeasure = product.Unit ?? "pcs",
                ReorderLevel = product.ReorderLevel,
                ReorderQuantity = product.MaxStockLevel ?? 0,
                IsActive = product.IsActive,
                Barcode = product.Barcode ?? "",
                Brand = product.Brand ?? "",
                Model = product.Model ?? ""
            });
        }

        return response;
    }

    public override async Task<ProductsResponse> GetProductsBySku(GetProductsBySkuRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetProductsBySku called for {Count} SKUs", request.Skus.Count);

        if (!Guid.TryParse(request.TenantId, out var tenantId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid tenant ID format"));
        }

        var response = new ProductsResponse();

        // Get all products and filter by SKU (since we don't have GetBySkuAsync method)
        var allProducts = await _productService.GetAllAsync(tenantId);
        var filteredProducts = allProducts.Where(p => request.Skus.Contains(p.Sku)).ToList();

        foreach (var product in filteredProducts)
        {
            response.Products.Add(new ProductResponse
            {
                Id = product.Id.ToString(),
                TenantId = product.TenantId.ToString(),
                Sku = product.Sku,
                Name = product.Name,
                Description = product.Description ?? "",
                CategoryId = product.CategoryId.ToString(),
                CategoryName = product.CategoryName ?? "",
                ProductType = product.ProductType ?? "PART",
                UnitPrice = (double)product.UnitPrice,
                CostPrice = product.CostPrice.HasValue ? (double)product.CostPrice.Value : 0,
                Currency = product.Currency ?? "PHP",
                UnitOfMeasure = product.Unit ?? "pcs",
                ReorderLevel = product.ReorderLevel,
                ReorderQuantity = product.MaxStockLevel ?? 0,
                IsActive = product.IsActive,
                Barcode = product.Barcode ?? "",
                Brand = product.Brand ?? "",
                Model = product.Model ?? ""
            });
        }

        response.TotalCount = response.Products.Count;
        return response;
    }

    public override async Task<ProductsResponse> GetLowStockProducts(GetLowStockProductsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetLowStockProducts called for tenant {TenantId}", request.TenantId);

        if (!Guid.TryParse(request.TenantId, out var tenantId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid tenant ID format"));
        }

        var products = await _productService.GetLowStockProductsAsync(tenantId);

        var response = new ProductsResponse
        {
            TotalCount = products.Count
        };

        foreach (var product in products)
        {
            response.Products.Add(new ProductResponse
            {
                Id = product.Id.ToString(),
                TenantId = product.TenantId.ToString(),
                Sku = product.Sku,
                Name = product.Name,
                Description = product.Description ?? "",
                CategoryId = product.CategoryId.ToString(),
                CategoryName = product.CategoryName ?? "",
                ProductType = product.ProductType ?? "PART",
                UnitPrice = (double)product.UnitPrice,
                CostPrice = product.CostPrice.HasValue ? (double)product.CostPrice.Value : 0,
                Currency = product.Currency ?? "PHP",
                UnitOfMeasure = product.Unit ?? "pcs",
                ReorderLevel = product.ReorderLevel,
                ReorderQuantity = product.MaxStockLevel ?? 0,
                IsActive = product.IsActive,
                Barcode = product.Barcode ?? "",
                Brand = product.Brand ?? "",
                Model = product.Model ?? ""
            });
        }

        return response;
    }

    public override async Task<ProductExistsResponse> ProductExists(ProductExistsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC ProductExists called for {ProductId}", request.ProductId);

        if (!Guid.TryParse(request.ProductId, out var productId))
        {
            return new ProductExistsResponse { Exists = false };
        }

        var product = await _productService.GetByIdAsync(productId);
        return new ProductExistsResponse { Exists = product != null };
    }

    public override async Task<StockAvailabilityResponse> CheckStockAvailability(CheckStockAvailabilityRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC CheckStockAvailability called for product {ProductId}", request.ProductId);

        if (!Guid.TryParse(request.ProductId, out var productId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid product ID format"));
        }

        Guid? branchId = null;
        if (!string.IsNullOrEmpty(request.BranchId) && Guid.TryParse(request.BranchId, out var bId))
        {
            branchId = bId;
        }

        var stockLevel = await _stockRepository.GetStockLevelAsync(productId, branchId);

        var availableQuantity = stockLevel?.AvailableQuantity ?? 0;
        var isAvailable = availableQuantity >= request.RequiredQuantity;

        return new StockAvailabilityResponse
        {
            Available = isAvailable,
            CurrentQuantity = availableQuantity,
            RequiredQuantity = request.RequiredQuantity,
            ProductId = request.ProductId,
            BranchId = request.BranchId ?? ""
        };
    }

    public override async Task<StockLevelResponse> GetStockLevel(GetStockLevelRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetStockLevel called for product {ProductId}", request.ProductId);

        if (!Guid.TryParse(request.ProductId, out var productId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid product ID format"));
        }

        Guid? branchId = null;
        if (!string.IsNullOrEmpty(request.BranchId) && Guid.TryParse(request.BranchId, out var bId))
        {
            branchId = bId;
        }

        var stockLevel = await _stockRepository.GetStockLevelAsync(productId, branchId);

        if (stockLevel == null)
        {
            return new StockLevelResponse
            {
                ProductId = request.ProductId,
                BranchId = request.BranchId ?? "",
                QuantityOnHand = 0,
                QuantityReserved = 0,
                QuantityAvailable = 0,
                ReorderLevel = 0,
                IsLowStock = true
            };
        }

        var product = await _productService.GetByIdAsync(productId);
        var reorderLevel = product?.ReorderLevel ?? 0;

        return new StockLevelResponse
        {
            ProductId = request.ProductId,
            BranchId = request.BranchId ?? "",
            QuantityOnHand = stockLevel.CurrentQuantity,
            QuantityReserved = stockLevel.ReservedQuantity, // Fixed line
            QuantityAvailable = stockLevel.AvailableQuantity,
            ReorderLevel = reorderLevel,
            IsLowStock = stockLevel.AvailableQuantity <= reorderLevel
        };
    }
}