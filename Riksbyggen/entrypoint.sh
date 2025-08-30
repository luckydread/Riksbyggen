#!/bin/bash
set -e

echo "Waiting for SQL Server to start..."
sleep 30  # adjust this depending on your droplet performance

echo "Starting .NET app..."
dotnet Riksbyggen.dll

