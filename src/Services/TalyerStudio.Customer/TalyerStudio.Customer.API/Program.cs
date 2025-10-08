using Microsoft.EntityFrameworkCore;
using TalyerStudio.Customer.API.Services;
using TalyerStudio.Customer.Application.Interfaces;
using TalyerStudio.Customer.Application.Services;
using TalyerStudio.Customer.Infrastructure.Data;
using TalyerStudio.Customer.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TalyerStudio Customer API", Version = "v1" });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add gRPC
builder.Services.AddGrpc();

// Database
builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection - Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

// Dependency Injection - Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
builder.Services.AddScoped<IServiceService, ServiceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Map gRPC services
app.MapGrpcService<CustomerGrpcService>();
app.MapGrpcService<ServiceCatalogGrpcService>();

// Enable gRPC-Web for browser clients (optional)
// app.UseGrpcWeb();

app.Run();