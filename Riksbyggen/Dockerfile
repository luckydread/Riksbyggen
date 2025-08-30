# -----------------------
# Build stage
# -----------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore "Riksbyggen.csproj"

# Copy remaining source code
COPY . .

# Build and publish
RUN dotnet build "Riksbyggen.csproj" -c Release -o /app/build
RUN dotnet publish "Riksbyggen.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Install dotnet-ef globally
RUN dotnet tool install --global dotnet-ef \
    && export PATH="$PATH:/root/.dotnet/tools"
ENV PATH="$PATH:/root/.dotnet/tools"
# -----------------------
# Runtime stage
# -----------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS runtime
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Copy entrypoint script
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

# Expose API port
EXPOSE 5000

# Start container via entrypoint
ENTRYPOINT ["./entrypoint.sh"]

