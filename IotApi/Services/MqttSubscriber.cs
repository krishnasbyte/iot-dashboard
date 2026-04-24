using MQTTnet;
using MQTTnet.Client;
using System.Text;
using System.Text.Json;
using IotApi.Models;

namespace IotApi.Services;

public class MqttSubscriber : BackgroundService
{
    private readonly ILogger<MqttSubscriber> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private IMqttClient? _mqttClient;

    public MqttSubscriber(ILogger<MqttSubscriber> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MQTT Subscriber starting...");

        var factory = new MqttFactory();
        _mqttClient = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883)
            .WithClientId("iot-api")
            .WithCleanSession()
            .Build();

        if (_mqttClient != null)
        {
            _mqttClient.ApplicationMessageReceivedAsync += OnMessageReceived;

            try
            {
                await _mqttClient.ConnectAsync(options, stoppingToken);
                await _mqttClient.SubscribeAsync("sensors/data");
                _logger.LogInformation("Connected to MQTT broker and subscribed to 'sensors/data'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to MQTT broker");
            }
        }
    }

    private async Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs args)
    {
        try
        {
            var payload = Encoding.UTF8.GetString(args.ApplicationMessage.PayloadSegment);
            _logger.LogDebug("Received MQTT message: {Payload}", payload);

            var sensorData = JsonSerializer.Deserialize<SensorData>(payload);
            if (sensorData != null)
            {
                sensorData.ReceivedAt = DateTime.UtcNow;
                
                using var scope = _scopeFactory.CreateScope();
                var influxDbService = scope.ServiceProvider.GetRequiredService<InfluxDbService>();
                influxDbService.WriteSensorData(sensorData);
                
                _logger.LogInformation("Stored sensor data from {DeviceId}", sensorData.DeviceId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing MQTT message");
        }
    }
}
