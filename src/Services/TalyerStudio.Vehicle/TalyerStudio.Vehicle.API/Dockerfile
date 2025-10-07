FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["TalyerStudio.sln", "./"]

# Copy all csproj files
COPY ["Services/TalyerStudio.Vehicle/TalyerStudio.Vehicle.API/TalyerStudio.Vehicle.API.csproj", "Services/TalyerStudio.Vehicle/TalyerStudio.Vehicle.API/"]
COPY ["Services/TalyerStudio.Vehicle/TalyerStudio.Vehicle.Application/TalyerStudio.Vehicle.Application.csproj", "Services/TalyerStudio.Vehicle/TalyerStudio.Vehicle.Application/"]
COPY ["Services/TalyerStudio.Vehicle/TalyerStudio.Vehicle.Domain/TalyerStudio.Vehicle.Domain.csproj", "Services/TalyerStudio.Vehicle/TalyerStudio.Vehicle.Domain/"]
COPY ["Services/TalyerStudio.Vehicle/TalyerStudio.Vehicle.Infrastructure/TalyerStudio.Vehicle.Infrastructure.csproj", "Services/TalyerStudio.Vehicle/TalyerStudio.Vehicle.Infrastructure/"]
COPY ["Shared/TalyerStudio.Shared.Contracts/TalyerStudio.Shared.Contracts.csproj", "Shared/TalyerStudio.Shared.Contracts/"]
COPY ["Shared/TalyerStudio.Shared.Events/TalyerStudio.Shared.Events.csproj", "Shared/TalyerStudio.Shared.Events/"]
COPY ["Shared/TalyerStudio.Shared.Infrastructure/TalyerStudio.Shared.Infrastructure.csproj", "Shared/TalyerStudio.Shared.Infrastructure/"]

# Restore
RUN dotnet restore "Services/TalyerStudio.Vehicle/TalyerStudio.Vehicle.API/TalyerStudio.Vehicle.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/Services/TalyerStudio.Vehicle/TalyerStudio.Vehicle.API"
RUN dotnet build "TalyerStudio.Vehicle.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TalyerStudio.Vehicle.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TalyerStudio.Vehicle.API.dll"]
