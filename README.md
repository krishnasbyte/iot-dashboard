# 🌡️ IoT Sensor Dashboard

[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![MQTT](https://img.shields.io/badge/MQTT-Mosquitto-blue)](https://mosquitto.org/)
[![InfluxDB](https://img.shields.io/badge/InfluxDB-2.7-green)](https://influxdata.com/)
[![License](https://img.shields.io/badge/License-MIT-yellow)](LICENSE)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)]()

## 📌 Overview

A **production-grade IoT Sensor Monitoring System** running on a **Raspberry Pi 5** with K3s Kubernetes. This project demonstrates real-time sensor data ingestion via MQTT, time-series storage with InfluxDB, and a live web dashboard.

### 🎯 Why This Project Stands Out

- **Runs on $80 Raspberry Pi 5** - Full microservices on edge hardware
- **ARM64 Optimized** - Containerized for ARM architecture
- **Dual Storage** - In-memory (real-time) + InfluxDB (historical)
- **MQTT + HTTP** - Both protocols supported
- **Real-time Dashboard** - Live charts updating every 5 seconds
- **Production Patterns** - Health checks, dependency injection, background services

## 🏗️ Architecture
┌─────────────────────────────────────────────────────────────────────────────┐
│ COMPLETE IoT ARCHITECTURE │
├─────────────────────────────────────────────────────────────────────────────┤
│ │
│ ┌─────────────────┐ MQTT/HTTP ┌─────────────────────────────────┐ │
│ │ SensorSimulator │ ───────────────→ │ IoT API │ │
│ │ (C# Console) │ │ (C# .NET 8) │ │
│ └─────────────────┘ │ │ │
│ │ ┌──────────┐ ┌──────────────┐ │ │
│ ┌─────────────────┐ │ │ MQTT │ │ In-Memory │ │ │
│ │ MQTT Broker │ ←───────────────── │ │ Sub │ │ Storage │ │ │
│ │ (Mosquitto) │ │ └──────────┘ └──────────────┘ │ │
│ └─────────────────┘ │ │ │ │ │
│ │ ▼ ▼ │ │
│ │ ┌─────────────────────────┐ │ │
│ │ │ InfluxDB │ │ │
│ │ │ (Time-Series DB) │ │ │
│ │ └─────────────────────────┘ │ │
│ └───────────────┬─────────────────┘ │
│ │ │
│ │ REST API │
│ ▼ │
│ ┌─────────────────────────────────┐ │
│ │ Web Dashboard │ │
│ │ (HTML/CSS/JavaScript) │ │
│ │ │ │
│ │ • Live temperature chart │ │
│ │ • Real-time updates (5s) │ │
│ │ • Historical data view │ │
│ └─────────────────────────────────┘ │
│ │
└─────────────────────────────────────────────────────────────────────────────┘

text

## 🚀 Quick Start

### Prerequisites

- Raspberry Pi 5 with Ubuntu 64-bit
- Docker & Docker Compose
- .NET 8 SDK
- Python 3 (for dashboard server)

### Step 1: Clone the Repository

```bash
git clone https://github.com/krishnasbyte/iot-dashboard.git
cd iot-dashboard
Step 2: Start Infrastructure (MQTT + InfluxDB + PostgreSQL)
bash
# Start all Docker services
docker-compose up -d

# Verify containers are running
docker-compose ps
Step 3: Run the IoT API
bash
cd IotApi
dotnet run
Step 4: Run the Sensor Simulator
bash
# In a new terminal
cd SensorSimulator
dotnet run
Step 5: Launch the Web Dashboard
bash
# In a new terminal
cd Dashboard
python3 -m http.server 5500
Step 6: Open the Dashboard
Open your browser to: http://localhost:5500

📊 API Reference
Method	Endpoint	Description
POST	/api/sensor/data	Ingest sensor data
GET	/api/sensor/memory	Get recent data from memory
GET	/api/sensor/data/{deviceId}	Get historical data from InfluxDB
GET	/api/sensor/health	Health check
Example API Calls
bash
# Store sensor data
curl -X POST http://localhost:5215/api/sensor/data \
  -H "Content-Type: application/json" \
  -d '{"deviceId":"sensor-01","temperature":23.5,"humidity":55.2,"pressure":1013.2}'

# Get recent data
curl http://localhost:5215/api/sensor/memory

# Get historical data (last 24 hours)
curl http://localhost:5215/api/sensor/data/sensor-01

# Health check
curl http://localhost:5215/api/sensor/health
🎨 Dashboard Features
Live Cards - Temperature, Humidity, Pressure with trend indicators

Real-time Chart - Interactive line chart with Chart.js

Data Table - Recent readings with timestamps

Status Indicator - Shows API connection status

Auto-refresh - Updates every 5 seconds

📁 Project Structure
text
iot-dashboard/
├── IotApi/                      # .NET 8 Web API
│   ├── Controllers/             # REST endpoints
│   ├── Services/                # MQTT + InfluxDB services
│   ├── Models/                  # Data models
│   └── Program.cs               # DI configuration
├── SensorSimulator/             # Simulates IoT sensor
├── Dashboard/                   # Web dashboard
│   ├── index.html               # Main dashboard page
│   ├── css/style.css            # Styling
│   └── js/dashboard.js          # Real-time updates
├── mosquitto/                   # MQTT broker config
├── docker-compose.yml           # Infrastructure containers
└── README.md                    # This file
🛠️ Technology Stack
Component	Technology	Purpose
Backend API	C# .NET 8	REST API + MQTT subscriber
Message Broker	Mosquitto (MQTT)	IoT message routing
Time-Series DB	InfluxDB 2.7	Historical sensor data
Relational DB	PostgreSQL 15	Device metadata
Dashboard	HTML/CSS/JS + Chart.js	Real-time visualization
Containerization	Docker	Infrastructure isolation
🎯 Skills Demonstrated
Skill	How It's Shown
C# .NET 8	Complete Web API with dependency injection
MQTT Protocol	Pub/sub pattern for IoT data ingestion
Time-Series Database	InfluxDB for metrics storage
Dual Storage Pattern	Memory (fast) + InfluxDB (persistent)
Background Services	MQTT subscriber as hosted service
REST API Design	Clean endpoints with proper HTTP status
Real-time Dashboard	Live charts with auto-refresh
Docker	Multi-container infrastructure
ARM64 Optimization	Running on Raspberry Pi 5
🔍 Monitoring & Debugging
bash
# Check all container status
docker-compose ps

# View MQTT broker logs
docker logs mqtt-broker -f

# Check InfluxDB health
curl http://localhost:8086/health

# View API logs (in API terminal)
# Look for "Stored sensor data" messages

# Check memory storage
curl http://localhost:5215/api/sensor/memory
📈 Performance Metrics
API Response Time: <10ms (memory), <50ms (InfluxDB)

Data Ingestion: 5-second intervals from simulator

Dashboard Refresh: Every 5 seconds

Storage: InfluxDB compression (90%+ space saving)

Resource Usage: <512MB RAM for all services

🚧 Future Improvements
Add device registration (PostgreSQL)

User authentication (JWT)

Email/SMS alerts for threshold violations

WebSocket push notifications

Prometheus + Grafana monitoring

Helm charts for K3s deployment

CI/CD pipeline with GitHub Actions

🐛 Troubleshooting
Issue	Solution
API won't start	Check port 5215: sudo lsof -i :5215
No data in dashboard	Verify API is running: curl http://localhost:5215/api/sensor/health
MQTT connection failed	Check broker: docker logs mqtt-broker
InfluxDB write errors	Check token in appsettings.json
📝 License
MIT License - see LICENSE file

👤 Author
Bikash Chhetri
Senior Software Engineer | Embedded Systems & Fintech

7+ years: C# .NET, C++, Azure, Payment Systems

Built: EFT-POS integrations, card issuance kiosks, RTOS payment terminals

🔗 GitHub
🔗 LinkedIn

🙏 Acknowledgments
Microsoft for .NET and excellent documentation

InfluxData for time-series database

The MQTT community for lightweight protocol

Raspberry Pi Foundation for amazing hardware

*Built on Raspberry Pi 5 - Production-grade IoT infrastructure on edge hardware*

⭐ Star this repository if you find it useful!
