# 🌡️ IoT Sensor Dashboard - Complete Dockerized System

[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![MQTT](https://img.shields.io/badge/MQTT-Mosquitto-blue)](https://mosquitto.org/)
[![InfluxDB](https://img.shields.io/badge/InfluxDB-2.7-green)](https://influxdata.com/)
[![Docker](https://img.shields.io/badge/Docker-6%20Services-blue)](https://docker.com/)
[![License](https://img.shields.io/badge/License-MIT-yellow)](LICENSE)

## 📌 Overview

A **production-grade IoT Sensor Monitoring System** fully containerized and running on a **Raspberry Pi 5**. This project demonstrates a complete end-to-end IoT solution with real-time data visualization.

### 🎯 Key Features

- ✅ **6 Docker Containers** - Full containerization with docker-compose
- ✅ **MQTT Protocol** - Lightweight messaging for IoT devices
- ✅ **Time-Series Database** - InfluxDB for efficient metrics storage
- ✅ **Real-time Dashboard** - Live charts with Chart.js
- ✅ **RESTful API** - Clean .NET 8 API with health checks
- ✅ **ARM64 Optimized** - Runs efficiently on Raspberry Pi 5
- ✅ **Health Checks** - Production-ready container monitoring

## 🏗️ System Architecture

┌─────────────────────────────────────────────────────────────────────────────┐\
│ DOCKERIZED IoT SYSTEM │\
│ (Raspberry Pi 5) │\
├─────────────────────────────────────────────────────────────────────────────┤\
│ │\
│ ┌──────────────┐ ┌──────────────┐ ┌──────────────┐ │\
│ │ Mosquitto │ │ InfluxDB │ │ PostgreSQL │ │\
│ │ (MQTT) │ │ :8086 │ │ :5433 │ │\
│ │ :1883 │ │ │ │ │ │\
│ └──────┬───────┘ └──────┬───────┘ └──────┬───────┘ │\
│ │ │ │ │\
│ └───────────────────┼───────────────────┘ │\
│ │ │\
│ ▼ │\
│ ┌──────────────┐ │\
│ │ IoT API │ │\
│ │ :5215 │ │\
│ │ (.NET 8) │ │\
│ └──────┬───────┘ │\
│ │ │\
│ ┌────────────┼────────────┐ │\
│ ▼ ▼ ▼ │\
│ ┌──────────┐ ┌──────────┐ ┌──────────┐ │\
│ │Simulator │ │Dashboard │ │ InfluxDB │ │\
│ │ (Sensor │ │ :5500 │ │ Storage │ │\
│ │ Data) │ │ │ │ │ │\
│ └──────────┘ └──────────┘ └──────────┘ │\
│ │\
│ ✅ One command starts all 6 containers │\
│ ✅ Automatic health checks │\
│ ✅ Container orchestration with docker-compose │\
│ │\
└─────────────────────────────────────────────────────────────────────────────┘

text

## 🚀 Quick Start

### Prerequisites

- Raspberry Pi 5 with Ubuntu 64-bit (or any Docker-supported system)
- Docker & Docker Compose
- Git

### One-Command Deployment

```bash
# Clone the repository
git clone https://github.com/krishnasbyte/iot-dashboard.git
cd iot-dashboard

# Start all 6 services
docker-compose up -d

# Check status
docker-compose ps

### Access Services

| Service | URL | Description |
| --- | --- | --- |
| Dashboard | [http://localhost:5500](http://localhost:5500/) | Real-time sensor visualization |
| API Health | <http://localhost:5215/api/sensor/health> | API health check |
| Sensor Data | <http://localhost:5215/api/sensor/memory> | View raw sensor data |
| InfluxDB | [http://localhost:8086](http://localhost:8086/) | Time-series database |
| MQTT | mqtt://localhost:1883 | MQTT broker |

📊 API Endpoints
----------------

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/sensor/health` | Health check |
| GET | `/api/sensor/memory` | Get recent data from memory |
| GET | `/api/sensor/data/{deviceId}` | Get historical data from InfluxDB |
| POST | `/api/sensor/data` | Ingest sensor data |

### Example API Call

bash

# Ingest sensor data
curl -X POST http://localhost:5215/api/sensor/data\
  -H "Content-Type: application/json"\
  -d '{"deviceId":"sensor-01","temperature":23.5,"humidity":55.2,"pressure":1013.2}'

# Query latest data
curl http://localhost:5215/api/sensor/memory

🐳 Docker Services
------------------

| Container | Purpose | Port |
| --- | --- | --- |
| `iot-mosquitto` | MQTT Broker | 1883, 9001 |
| `iot-influxdb` | Time-series database | 8086 |
| `iot-postgres` | Device metadata | 5433 |
| `iot-api` | .NET 8 REST API | 5215 |
| `iot-dashboard` | Web UI (Nginx) | 5500 |
| `iot-simulator` | Sensor data generator | - |

### Docker Commands

bash

# View all containers
docker-compose ps

# View logs
docker-compose logs -f

# View specific service logs
docker-compose logs -f iot-api
docker-compose logs -f simulator

# Stop all services
docker-compose down

# Restart all services
docker-compose restart

# Rebuild after changes
docker-compose build --no-cache
docker-compose up -d

📁 Project Structure
--------------------

text

iot-dashboard/
├── IotApi/                      # .NET 8 Web API
│   ├── Controllers/             # REST endpoints
│   ├── Services/                # MQTT + InfluxDB services
│   ├── Models/                  # Data models
│   ├── Program.cs               # Configuration with CORS
│   └── Dockerfile               # ARM64 optimized
├── Dashboard/                   # Web Dashboard
│   ├── index.html               # Main dashboard
│   ├── css/style.css            # Styling
│   ├── js/dashboard.js          # Real-time updates
│   ├── nginx.conf               # Nginx proxy config
│   └── Dockerfile               # Nginx container
├── SensorSimulator/             # Data generator
│   ├── Program.cs               # Simulates sensor data
│   └── Dockerfile               # .NET console container
├── mosquitto/config/            # MQTT configuration
├── docker-compose.yml           # 6-service orchestration
└── README.md                    # This file

🔧 Configuration
----------------

### Environment Variables

| Variable | Default | Description |
| --- | --- | --- |
| `DEVICE_ID` | sensor-01 | Simulator device ID |
| `API_URL` | [http://iot-api:5215](http://iot-api:5215/) | API endpoint for simulator |
| `InfluxDB__Token` | my-super-secret-token-123 | InfluxDB auth token |

### Ports

| Service | Host Port | Container Port |
| --- | --- | --- |
| MQTT | 1883 | 1883 |
| MQTT WebSocket | 9001 | 9001 |
| InfluxDB | 8086 | 8086 |
| PostgreSQL | 5433 | 5432 |
| IoT API | 5215 | 5215 |
| Dashboard | 5500 | 80 |

📈 Dashboard Features
---------------------

-   Live Cards - Temperature, humidity, pressure with trend indicators

-   Real-time Chart - Interactive line chart using Chart.js

-   Data Table - Recent readings with timestamps

-   Status Indicator - Shows API connection status

-   Auto-refresh - Updates every 5 seconds

🛠️ Technology Stack
--------------------

| Component | Technology | Purpose |
| --- | --- | --- |
| Backend API | C# .NET 8 | REST API + MQTT subscriber |
| Message Broker | Mosquitto | MQTT message routing |
| Time-Series DB | InfluxDB 2.7 | Sensor data storage |
| Relational DB | PostgreSQL 15 | Device metadata |
| Web Dashboard | HTML/CSS/JS + Chart.js | Real-time visualization |
| Reverse Proxy | Nginx | API proxy for dashboard |
| Containerization | Docker + Compose | 6-service orchestration |

🎯 Skills Demonstrated
----------------------

-   ✅ C# .NET 8 - Web API with dependency injection

-   ✅ MQTT Protocol - Pub/sub pattern for IoT

-   ✅ Time-Series Database - InfluxDB for metrics

-   ✅ Docker - Multi-container orchestration

-   ✅ ARM64 Optimization - Raspberry Pi 5 deployment

-   ✅ REST API Design - Clean endpoints with Swagger

-   ✅ Real-time Dashboard - Live charts with Chart.js

🔍 Troubleshooting
------------------

### Check container status

bash

docker-compose ps

### View API logs

bash

docker-compose logs iot-api

### Test API connectivity

bash

curl http://localhost:5215/api/sensor/health

### Restart a specific service

bash

docker-compose restart iot-api

📝 License
----------

MIT License - see [LICENSE](https://license/) file

👤 Author
---------

Bikash Chhetri\
Senior Software Engineer | Embedded Systems & Fintech

-   7+ years: C# .NET, C++, Azure, Payment Systems

-   Built: EFT-POS integrations, card issuance kiosks, RTOS payment terminals

[](https://github.com/krishnasbyte)<https://img.shields.io/badge/GitHub-krishnasbyte-181717?style=flat&logo=github>\
[](https://linkedin.com/in/bikash-chhetri)<https://img.shields.io/badge/LinkedIn-Bikash%2520Chhetri-0077B5?style=flat&logo=linkedin>

* * * * *

🌟 Star This Repository
-----------------------

If you find this project useful, please give it a star! ⭐

* * * * *

*Built on Raspberry Pi 5 - Production-grade IoT infrastructure running entirely in Docker containers* 🚀\
