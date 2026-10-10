using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Redis Bağlantısı (Docker Compose servis adı: 'redis-cache')
var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? "redis-cache:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConnectionString));

builder.Services.AddHealthChecks();
var app = builder.Build();

app.MapHealthChecks("/health");

app.MapGet("/api/products", async ([FromServices] IConnectionMultiplexer redis) =>
{
    var db = redis.GetDatabase();
    string cacheKey = "products_list_cache";
    
    // 1. Önce Redis Cache'e bak
    var cachedData = await db.StringGetAsync(cacheKey);
    if (!cachedData.IsNull)
    {
        return Results.Ok(new
        {
            Source = "Redis Cache (.NET 10)",
            NodeId = Environment.MachineName,
            Data = cachedData.ToString()
        });
    }

    // 2. Cache'te yoksa (DB Simülasyonu)
    var freshData = "Laptop, Mouse, Keyboard, Monitor (Fetched from DB)";
    
    // 3. Redis'e 30 saniyeliğine kaydet
    await db.StringSetAsync(cacheKey, freshData, TimeSpan.FromSeconds(30));

    return Results.Ok(new
    {
        Source = "Database (Cache Miss - .NET 10)",
        NodeId = Environment.MachineName,
        Data = freshData
    });
});

app.Run("http://+:8080");