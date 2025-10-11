FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["TalyerStudio.sln", "./"]

# Copy all csproj files for Inventory Service
COPY ["Services/TalyerStudio.Inventory/TalyerStudio.Inventory.API/TalyerStudio.Inventory.API.csproj", "Services/TalyerStudio.Inventory/TalyerStudio.Inventory.API/"]
COPY ["Services/TalyerStudio.Inventory/TalyerStudio.Inventory.Application/TalyerStudio.Inventory.Application.csproj", "Services/TalyerStudio.Inventory/TalyerStudio.Inventory.Application/"]
COPY ["Services/TalyerStudio.Inventory/TalyerStudio.Inventory.Domain/TalyerStudio.Inventory.Domain.csproj", "Services/TalyerStudio.Inventory/TalyerStudio.Inventory.Domain/"]
COPY ["Services/TalyerStudio.Inventory/TalyerStudio.Inventory.Infrastructure/TalyerStudio.Inventory.Infrastructure.csproj", "Services/TalyerStudio.Inventory/TalyerStudio.Inventory.Infrastructure/"]

# Copy shared projects
COPY ["Shared/TalyerStudio.Shared.Contracts/TalyerStudio.Shared.Contracts.csproj", "Shared/TalyerStudio.Shared.Contracts/"]
COPY ["Shared/TalyerStudio.Shared.Events/TalyerStudio.Shared.Events.csproj", "Shared/TalyerStudio.Shared.Events/"]
COPY ["Shared/TalyerStudio.Shared.Infrastructure/TalyerStudio.Shared.Infrastructure.csproj", "Shared/TalyerStudio.Shared.Infrastructure/"]

# Restore
RUN dotnet restore "Services/TalyerStudio.Inventory/TalyerStudio.Inventory.API/TalyerStudio.Inventory.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/Services/TalyerStudio.Inventory/TalyerStudio.Inventory.API"
RUN dotnet build "TalyerStudio.Inventory.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TalyerStudio.Inventory.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TalyerStudio.Inventory.API.dll"]