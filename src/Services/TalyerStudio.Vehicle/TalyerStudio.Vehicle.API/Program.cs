using TalyerStudio.Shared.Infrastructure.Grpc;
using Grpc.Net.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Add gRPC Client for Customer Service
builder.Services.AddGrpcClient<CustomerService.CustomerServiceClient>(options =>
{
    // Use dedicated gRPC port
    options.Address = new Uri("http://localhost:5147");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    // For development only - allows HTTP/2 without TLS
    handler.ServerCertificateCustomValidationCallback = 
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return handler;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS (must be before UseAuthorization)
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();