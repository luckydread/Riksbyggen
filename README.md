# 🏢 Riksbyggen Property Management App

A property management system with a **.NET 9 API backend**, **React + Vite frontend**, and **SQL Server 2019 database**.  
It can be run on a server using Docker (e.g., DigitalOcean Droplet) or locally for development.

---

## 📑 Table of Contents

- [Live Access](#live-access)
- [API Endpoints](#api-endpoints)
- [Architecture](#architecture)
- [Docker Deployment](#docker-deployment)
- [Local Development](#local-development)
- [Environment Variables](#environment-variables)

---

## 🌍 Live Access

Deployed on the droplet at:

- **Frontend:** http://138.68.79.101:8080  
- **Backend API:** http://138.68.79.101:5000  

---

## 🔗 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| **POST** | `/api/Companies` | Create a new company |
| **POST** | `/Apartment` | Create a new apartment |
| **GET**  | `/api/Companies` | List all companies |
| **GET**  | `/api/Companies/{id}/apartments` | List apartments for a company |
| **GET**  | `/Apartment/{id}/expiring-leases` | Get apartments with leases expiring in 3 months |
| **PUT**  | `/Apartment/{id}/status` | Update apartment status |

Example – update apartment status:
```bash
curl -X PUT http://138.68.79.101:5000/Apartment/1/status \
  -H "Content-Type: application/json" \
  -H "X-Webhook-Secret: YOUR_WEBHOOK_SECRET" \
  -d '{"status":"Busy"}'

🗂️ Architecture

SQL Server 2019 – relational database

.NET API – backend with EF Core + REST endpoints

React + Vite frontend – served via Nginx

Docker Networks:

backend: SQL Server + API

frontend: React frontend + API

Docker Deployment

# Create networks
docker network create backend
docker network create frontend

# Start SQL Server
docker run -d \
  --name sqlserver2019 \
  --network backend \
  -e "ACCEPT_EULA=Y" \
  -e "SA_PASSWORD=Mhondoro!6" \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2019-latest

# Start .NET API
docker run -d \
  --name riksbyggen-api \
  --network backend \
  -p 5000:5000 \
  -e "DB_HOST=sqlserver2019" \
  -e "DB_NAME=RiksbyggenDb" \
  -e "DB_SA_PASSWORD=Mhondoro!6" \
  -e "WEBHOOK_SECRET=RandoMSecret-Webhook-Key" \
  -e "ASPNETCORE_URLS=http://+:5000" \
  riksbyggen-app

# Connect API also to frontend network
docker network connect frontend riksbyggen-api

# Start React Frontend
docker run -d \
  --name riksbyggen-frontend \
  --network frontend \
  -p 8080:80 \
  riksbyggen-frontend

  💻 Local Development
Requirements

.NET 9 SDK
Nodjs
Npm/yarn
SQL Server

Steps:

# 1. Setup Database
# Create database "RiksbyggenDb" in your SQL Server
# Then update .env or appsettings.Development.json with DB credentials
cd Riksbyggen
dotnet ef database update

# 2. Run .NET API
cd Riksbyggen
dotnet run

# 3. Run React Frontend
cd Dashboard
npm install
npm run dev

Environment Variables

| Variable          | Description                           |
| ----------------- | ------------------------------------- |
| `DB_HOST`         | SQL Server hostname                   |
| `DB_NAME`         | Database name                         |
| `DB_SA_PASSWORD`  | SQL Server SA password                |
| `WEBHOOK_SECRET`  | Secret key for webhook authentication |
| `ASPNETCORE_URLS` | URLs for .NET app to listen on        |
