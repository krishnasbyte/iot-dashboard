using System.Text;
using System.Text.Json;

Console.WriteLine("🌡️ IoT Sensor Simulator Starting...");
Console.WriteLine("=================================");

var deviceId = Environment.GetEnvironmentVariable("DEVICE_ID") ?? "sensor-01";
var apiUrl = Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost:5215";

Console.WriteLine($"Device ID: {deviceId}");
Console.WriteLine($"API URL: {apiUrl}");
Console.WriteLine("Press Ctrl+C to stop\n");

var random = new Random();
var httpClient = new HttpClient();
var messageCount = 0;

while (true)
{
    try
    {
        var sensorData = new
        {
            deviceId = deviceId,  // ← This is now included!
            temperature = Math.Round(random.NextDouble() * 20 + 15, 1),
            humidity = Math.Round(random.NextDouble() * 50 + 30, 1),
            pressure = Math.Round(random.NextDouble() * 30 + 1000, 1),
            timestamp = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(sensorData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await httpClient.PostAsync($"{apiUrl}/api/sensor/data", content);
        
        if (response.IsSuccessStatusCode)
        {
            messageCount++;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"#{messageCount} - ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{deviceId}: ");
            Console.ResetColor();
            Console.WriteLine($"🌡️ {sensorData.temperature}°C  💧 {sensorData.humidity}%  📊 {sensorData.pressure}hPa");
        }
        else
        {
            Console.WriteLine($"❌ API Error: {response.StatusCode}");
        }

        await Task.Delay(5000);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error: {ex.Message}");
        await Task.Delay(5000);
    }
}
