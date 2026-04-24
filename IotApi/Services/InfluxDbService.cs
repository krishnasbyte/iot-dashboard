using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using IotApi.Models;

namespace IotApi.Services;

public class InfluxDbService
{
    private readonly ILogger<InfluxDbService> _logger;
    private readonly InfluxDBClient _client;
    private readonly string _bucket;
    private readonly string _org;

    public InfluxDbService(IConfiguration configuration, ILogger<InfluxDbService> logger)
    {
        _logger = logger;
        _bucket = configuration["InfluxDB:Bucket"] ?? "iot_metrics";
        _org = configuration["InfluxDB:Org"] ?? "my-org";
        
        var url = configuration["InfluxDB:Url"] ?? "http://localhost:8086";
        var token = configuration["InfluxDB:Token"] ?? "my-super-secret-token-123";
        
        _client = new InfluxDBClient(url, token);
        _logger.LogInformation("📊 InfluxDB Service initialized at {Url}", url);
    }

    public void WriteSensorData(SensorData data)
    {
        try
        {
            var point = PointData.Measurement("sensor_readings")
                .Tag("device_id", data.DeviceId ?? "unknown")
                .Field("temperature", data.Temperature)
                .Field("humidity", data.Humidity)
                .Field("pressure", data.Pressure)
                .Field("air_quality", data.AirQuality)
                .Timestamp(data.Timestamp, WritePrecision.Ms);

            using var writeApi = _client.GetWriteApi();
            writeApi.WritePoint(point, _bucket, _org);
            
            _logger.LogInformation("✅ Written to InfluxDB: {DeviceId} - Temp:{Temp}°C", data.DeviceId, data.Temperature);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Failed to write to InfluxDB");
        }
    }

    public async Task<List<SensorData>> QuerySensorDataAsync(string deviceId, int hours = 24)
    {
        var results = new List<SensorData>();

        var fluxQuery = $@"
            from(bucket: ""{_bucket}"")
            |> range(start: -{hours}h)
            |> filter(fn: (r) => r._measurement == ""sensor_readings"")
            |> filter(fn: (r) => r.device_id == ""{deviceId}"")
            |> pivot(rowKey:[""_time""], columnKey:[""_field""], valueColumn:""_value"")
            |> sort(columns: [""_time""], desc: false)
        ";

        try
        {
            _logger.LogInformation("📊 Querying InfluxDB for device {DeviceId}", deviceId);
            
            var tables = await _client.GetQueryApi().QueryAsync(fluxQuery, _org);
            
            foreach (var table in tables)
            {
                foreach (var record in table.Records)
                {
                    try
                    {
                        var data = new SensorData
                        {
                            DeviceId = deviceId,
                            Timestamp = record.GetTime()?.ToDateTimeUtc() ?? DateTime.UtcNow,
                            Temperature = record.GetValueByKey("temperature") as double? ?? 0,
                            Humidity = record.GetValueByKey("humidity") as double? ?? 0,
                            Pressure = record.GetValueByKey("pressure") as double? ?? 0,
                            AirQuality = Convert.ToInt32(record.GetValueByKey("air_quality") ?? 0),
                            ReceivedAt = DateTime.UtcNow
                        };
                        results.Add(data);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error parsing record");
                    }
                }
            }
            
            _logger.LogInformation("✅ Retrieved {Count} records from InfluxDB", results.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Failed to query InfluxDB");
        }

        return results;
    }
}
