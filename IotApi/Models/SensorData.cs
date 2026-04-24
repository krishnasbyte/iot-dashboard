namespace IotApi.Models;

public class SensorData
{
    public string? Id { get; set; }
    public string? DeviceId { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double Pressure { get; set; }
    public int AirQuality { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime ReceivedAt { get; set; }
}
