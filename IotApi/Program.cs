using IotApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure to listen on port 5215
builder.WebHost.UseUrls("http://0.0.0.0:5215");

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<InfluxDbService>();
builder.Services.AddHostedService<MqttSubscriber>();

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "IoT API Running! Port 5215");

app.Run();
