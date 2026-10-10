using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Json;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient("ApiClient", client =>
{
    var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://nginx-proxy:80";
    client.BaseAddress = new Uri(baseUrl);
})
.AddStandardResilienceHandler();

var host = builder.Build();
var clientFactory = host.Services.GetRequiredService<IHttpClientFactory>();
var client = clientFactory.CreateClient("ApiClient");

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

while (!cts.IsCancellationRequested)
{
    try
    {
        var response = await client.GetFromJsonAsync<ApiResponse>("/api/data", cts.Token);
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Yanıt Alındı -> Node: {response?.NodeId}");
    }
    catch (Exception ex) when (!cts.IsCancellationRequested)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Hata: {ex.Message}");
    }

    await Task.Delay(2000, cts.Token);
}

record ApiResponse(string Message, DateTime Timestamp, string NodeId);