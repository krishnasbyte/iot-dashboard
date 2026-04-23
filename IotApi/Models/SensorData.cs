using System.Text.Json.Serialization;

namespace IotApi.Models;

public class SensorData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [JsonPropertyName("device_id")]
    public string DeviceId { get; set; } = string.Empty;
    
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
    
    [JsonPropertyName("humidity")]
    public double Humidity { get; set; }
    
    [JsonPropertyName("pressure")]
    public double Pressure { get; set; }
    
    [JsonPropertyName("air_quality")]
    public int AirQuality { get; set; }
    
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
    
    [JsonPropertyName("received_at")]
    public DateTime ReceivedAt { get; set; }
}
