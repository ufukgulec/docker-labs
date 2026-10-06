var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => new { 
    Message = "Multi-Stage Build Başarılı!", 
    Timestamp = DateTime.UtcNow 
});

app.Run("http://0.0.0.0:8080");