var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

app.MapGet("/api/data", () => new
{
    Message = "Hello from Scaled .NET API Cluster!",
    Timestamp = DateTime.UtcNow,
    NodeId = Environment.MachineName // Yanıtı veren Container ID'si
});

app.Run("http://+:8080");