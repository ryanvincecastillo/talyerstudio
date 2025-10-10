using Microsoft.EntityFrameworkCore;
using TalyerStudio.JobOrder.API.Services;
using TalyerStudio.JobOrder.Application.Interfaces;
using TalyerStudio.JobOrder.Application.Services;
using TalyerStudio.JobOrder.Infrastructure.Data;
using TalyerStudio.JobOrder.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TalyerStudio JobOrder API", Version = "v1" });
});

// Add gRPC
builder.Services.AddGrpc();

// Database
builder.Services.AddDbContext<JobOrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<IJobOrderRepository, JobOrderRepository>();
builder.Services.AddScoped<IJobOrderService, JobOrderService>();

// CORS (for development)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Map gRPC service
app.MapGrpcService<JobOrderGrpcService>();

app.Run();