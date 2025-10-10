using Microsoft.EntityFrameworkCore;
using TalyerStudio.Invoice.API.Services;
using TalyerStudio.Invoice.Application.Interfaces;
using TalyerStudio.Invoice.Infrastructure.Data;
using TalyerStudio.Invoice.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "TalyerStudio Invoice API", 
        Version = "v1",
        Description = "Invoice and Payment Management API for TalyerStudio"
    });
});

// Add gRPC
builder.Services.AddGrpc();

// Configure PostgreSQL Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<InvoiceDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

// Configure CORS
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

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Map gRPC service
app.MapGrpcService<InvoiceGrpcService>();

app.Run();