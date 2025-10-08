using Microsoft.EntityFrameworkCore;
using TalyerStudio.Vehicle.API.Services;
using TalyerStudio.Vehicle.Application.Interfaces;
using TalyerStudio.Vehicle.Application.Services;
using TalyerStudio.Vehicle.Infrastructure.Data;
using TalyerStudio.Vehicle.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TalyerStudio Vehicle API", Version = "v1" });
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
builder.Services.AddDbContext<VehicleDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection - Repositories
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

// Dependency Injection - Services
builder.Services.AddScoped<IVehicleService, VehicleService>();

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
app.MapGrpcService<VehicleGrpcService>();

// Enable gRPC-Web for browser clients (optional)
// app.UseGrpcWeb();

app.Run();