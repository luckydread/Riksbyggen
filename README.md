# 🏢 Riksbyggen Property Management App

This is property management system with a **.NET 9 API backend**, **React + Vite frontend**, and **SQL Server 2019 database**.  

## 🌍 Live Access

The app is deployed on a linux droplet for demo purposes.

NOTE:The application will not be accessible after 2025-09-01

- **Frontend:** http://138.68.79.101:8080  
- **Backend API:** http://138.68.79.101:5000  

## 🔗 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| **POST** | `/api/Companies` | Create a new company |
| **POST** | `/Apartment` | Create a new apartment |
| **GET**  | `/api/Companies` | List all companies |
| **GET**  | `/api/Companies/{id}/apartments` | List apartments for a company |
| **GET**  | `/Apartment/{id}/expiring-leases` | Get apartments with leases expiring in 3 months |
| **PUT**  | `/Apartment/{id}/status` | Update apartment status |
 
 To run the app do the following:

# 1. Setup Database
# Create database "RiksbyggenDb" in your SQL Server
# Then creat a .env in you root folder and update it with the DB credentials below

cd Riksbyggen
dotnet ef database update

# 2. Run .NET API
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

Example command for testing webhook to update apartment status:

```bash
curl -X PUT http://138.68.79.101:5000/Apartment/1/status \
  -H "Content-Type: application/json" \
  -H "X-Webhook-Secret: YOUR_WEBHOOK_SECRET" \
  -d '{"status":"Busy"}'