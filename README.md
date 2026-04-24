# 🌡️ IoT Sensor Dashboard

[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![MQTT](https://img.shields.io/badge/MQTT-Mosquitto-blue)](https://mosquitto.org/)
[![InfluxDB](https://img.shields.io/badge/InfluxDB-2.7-green)](https://influxdata.com/)
[![License](https://img.shields.io/badge/License-MIT-yellow)](LICENSE)

## 📌 Overview

A production-grade **IoT Sensor Monitoring System** running on a Raspberry Pi 5 with K3s Kubernetes. This project demonstrates real-time sensor data ingestion via MQTT, time-series storage with InfluxDB, and REST API access.

### 🔥 Key Features

- ✅ **MQTT Broker (Mosquitto)** - Lightweight message routing for IoT sensors
- ✅ **Time-Series Database (InfluxDB)** - Optimized storage for sensor metrics
- ✅ **PostgreSQL** - Device metadata and user management
- ✅ **Sensor Simulator** - Generates realistic temperature, humidity, and pressure data
- ✅ **REST API** - Query sensor data with filtering and pagination
- ✅ **Docker Compose** - One-command local development
- ✅ **Kubernetes Ready** - Helm charts for K3s deployment

## 🏗️ Architecture
┌─────────────────────────────────────────────────────────────────┐
│ DATA FLOW │
├─────────────────────────────────────────────────────────────────┤
│ │
│ SensorSimulator → MQTT Broker → IoT API │
│ (C# Console) (Mosquitto) (C# .NET) │
│ │ │ │ │
│ │ │ ▼ │
│ Generates Routes Stores in │
│ fake sensor messages InfluxDB │
│ data every │
│ 5 seconds │
│ │
└─────────────────────────────────────────────────────────────────┘

text

## 🚀 Quick Start

### Prerequisites

- Docker & Docker Compose
- .NET 8 SDK
- MQTT Explorer (optional, for debugging)

### Step 1: Start Infrastructure

```bash
# Clone the repository
git clone https://github.com/krishnasbyte/iot-dashboard.git
cd iot-dashboard

# Start MQTT, InfluxDB, and PostgreSQL
docker-compose up -d

# Verify all containers are running
docker-compose ps
Step 2: Run Sensor Simulator
bash
# Run the sensor simulator
dotnet run --project SensorSimulator
Expected output:

text
🌡️ IoT Sensor Simulator Starting...
✅ Connected to MQTT Broker as sensor-01
[10:00:00] #1 - sensor-01: 🌡️ 23.5°C  💧 55.2%  📊 1013.2hPa
Step 3: Run IoT API
bash
# In a new terminal
cd IotApi
dotnet run
Step 4: Query Sensor Data
bash
# Health check
curl http://localhost:5000/api/sensor/health

# Get sensor data for last 24 hours
curl http://localhost:5000/api/sensor/data/sensor-01

# Get data for last 1 hour
curl http://localhost:5000/api/sensor/data/sensor-01?hours=1
📊 API Reference
Method	Endpoint	Description
GET	/api/sensor/health	Health check
GET	/api/sensor/devices	List all devices
GET	/api/sensor/data/{deviceId}	Get sensor data (default 24h)
GET	/api/sensor/data/{deviceId}?hours={n}	Get data for last n hours
🔧 Local Development
Infrastructure Ports
Service	Port	Purpose
Mosquitto (MQTT)	1883	MQTT protocol for sensors
Mosquitto (WebSocket)	9001	WebSocket for browsers
InfluxDB	8086	Time-series database API
PostgreSQL	5433	Relational database
IoT API	5000	REST API endpoint
Useful Commands
bash
# View MQTT messages
docker logs mqtt-broker -f

# Check InfluxDB health
curl http://localhost:8086/health

# Inspect PostgreSQL
docker exec -it iot-postgres psql -U admin -d iot_devices
📁 Project Structure
text
iot-dashboard/
├── IotApi/                 # Web API (MQTT subscriber + REST endpoints)
├── SensorSimulator/        # Simulates IoT sensor data
├── mosquitto/              # MQTT broker configuration
├── k8s/                    # Kubernetes manifests
├── helm/                   # Helm chart for K3s deployment
├── docker-compose.yml      # Local development environment
├── LICENSE                 # MIT License
└── README.md               # This file
🎯 Skills Demonstrated
✅ C# .NET 8 - Modern backend development

✅ MQTT Protocol - IoT messaging pattern

✅ Time-Series Database - InfluxDB for metrics

✅ Docker Compose - Multi-container orchestration

✅ REST API Design - Clean, documented endpoints

✅ Background Services - MQTT subscriber as hosted service

📈 Future Improvements
Web Dashboard with real-time charts

Device registration and management

Alerting system for threshold violations

WebSocket push notifications

Kubernetes deployment with Helm

👤 Author
Bikash Chhetri
Senior Software Engineer | Embedded Systems & Fintech

7+ years: C# .NET, C++, Azure, Payment Systems

Built: EFT-POS integrations, card issuance kiosks, RTOS payment terminals

🔗 GitHub
🔗 LinkedIn

📝 License
MIT License - see LICENSE file

*Built on Raspberry Pi 5 - Production-grade IoT infrastructure on edge hardware*
