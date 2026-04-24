using Microsoft.AspNetCore.Mvc;
using IotApi.Services;
using IotApi.Models;

namespace IotApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensorController : ControllerBase
{
    private readonly InfluxDbService _influxDbService;
    private readonly ILogger<SensorController> _logger;

    // In-memory fallback storage
    private static readonly List<SensorData> _memoryStorage = new();

    public SensorController(InfluxDbService influxDbService, ILogger<SensorController> logger)
    {
        _influxDbService = influxDbService;
        _logger = logger;
    }

    [HttpGet("data/{deviceId}")]
    public async Task<IActionResult> GetSensorData(string deviceId, [FromQuery] int hours = 24)
    {
        try
        {
            var data = await _influxDbService.QuerySensorDataAsync(deviceId, hours);
            return Ok(new { deviceId, hours, count = data.Count, data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving sensor data");
            return StatusCode(500, new { error = "Failed to retrieve sensor data" });
        }
    }

    [HttpPost("data")]
    public async Task<IActionResult> PostData([FromBody] SensorData data)
    {
        try
        {
            data.Id = Guid.NewGuid().ToString();
            data.Timestamp = DateTime.UtcNow;
            data.ReceivedAt = DateTime.UtcNow;
            
            // Store in memory (fallback)
            _memoryStorage.Add(data);
            
            // Store in InfluxDB
            _influxDbService.WriteSensorData(data);
            
            _logger.LogInformation("Data stored for device {DeviceId}: Temp={Temperature}°C", 
                data.DeviceId, data.Temperature);
            
            return Ok(new { 
                message = "Data received", 
                id = data.Id,
                temperature = data.Temperature,
                humidity = data.Humidity,
                pressure = data.Pressure
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error storing sensor data");
            return StatusCode(500, new { error = "Failed to store data" });
        }
    }

    [HttpGet("memory")]
    public IActionResult GetFromMemory()
    {
        return Ok(_memoryStorage.OrderByDescending(d => d.Timestamp).Take(50).ToList());
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { 
            status = "Healthy", 
            timestamp = DateTime.UtcNow,
            memoryCount = _memoryStorage.Count
        });
    }
}
