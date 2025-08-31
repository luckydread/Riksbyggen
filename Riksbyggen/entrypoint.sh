#!/bin/bash
set -e

echo "Waiting for SQL Server to start..."
sleep 30 

echo "Starting .NET app..."
dotnet Riksbyggen.dll
