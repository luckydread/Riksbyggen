#!/bin/sh
set -e

# Use the environment variable defined in docker-compose
echo "Waiting for SQL Server to be ready..."
until /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "$SA_PASSWORD" -Q 'SELECT 1' > /dev/null 2>&1; do
  echo "SQL Server not ready yet..."
  sleep 3
done

echo "SQL Server is ready. Running EF Core migrations..."
dotnet ef database update --no-build

echo "Starting API..."
exec dotnet Riksbyggen.dll
