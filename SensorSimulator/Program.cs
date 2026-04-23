using MQTTnet;
using MQTTnet.Client;
using System.Text;
using System.Text.Json;

Console.WriteLine("🌡️ IoT Sensor Simulator Starting...");
Console.WriteLine("=================================");

// Get device ID from environment or use default
var deviceId = Environment.GetEnvironmentVariable("DEVICE_ID") ?? "sensor-01";
var brokerHost = Environment.GetEnvironmentVariable("MQTT_BROKER") ?? "localhost";

Console.WriteLine($"Device ID: {deviceId}");
Console.WriteLine($"MQTT Broker: {brokerHost}:1883");
Console.WriteLine("Press Ctrl+C to stop\n");

var random = new Random();

// Create MQTT Client Factory
var factory = new MqttFactory();
var mqttClient = factory.CreateMqttClient();

var options = new MqttClientOptionsBuilder()
    .WithTcpServer(brokerHost, 1883)
    .WithClientId(deviceId)
    .WithCleanSession()
    .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
    .Build();

// Connect to MQTT Broker
try
{
    var connectResult = await mqttClient.ConnectAsync(options);
    
    if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
    {
        Console.WriteLine($"✅ Connected to MQTT Broker as {deviceId}\n");
    }
    else
    {
        Console.WriteLine($"❌ Failed to connect: {connectResult.ResultCode}");
        return;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Connection error: {ex.Message}");
    Console.WriteLine("Make sure MQTT broker is running: docker-compose up -d");
    return;
}

// Simulate sensor data
var messageCount = 0;
while (true)
{
    try
    {
        // Generate realistic sensor readings
        var sensorData = new
        {
            device_id = deviceId,
            temperature = Math.Round(random.NextDouble() * 20 + 15, 1),  // 15-35°C
            humidity = Math.Round(random.NextDouble() * 50 + 30, 1),     // 30-80%
            pressure = Math.Round(random.NextDouble() * 30 + 1000, 1),   // 1000-1030 hPa
            air_quality = random.Next(20, 150),                          // 20-150 PM2.5
            timestamp = DateTime.UtcNow
        };

        var payload = JsonSerializer.Serialize(sensorData);
        var message = new MqttApplicationMessageBuilder()
            .WithTopic("sensors/data")
            .WithPayload(Encoding.UTF8.GetBytes(payload))
            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
            .WithRetainFlag(false)
            .Build();

        await mqttClient.PublishAsync(message);
        messageCount++;

        // Colorful console output
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"#{messageCount} - ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"{deviceId}: ");
        Console.ResetColor();
        Console.WriteLine($"🌡️ {sensorData.temperature}°C  💧 {sensorData.humidity}%  📊 {sensorData.pressure}hPa");

        await Task.Delay(5000); // Send every 5 seconds
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Publish error: {ex.Message}");
        await Task.Delay(5000);
    }
}
