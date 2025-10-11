FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["TalyerStudio.sln", "./"]

# Copy all csproj files for Auth Service
COPY ["Services/TalyerStudio.Auth/TalyerStudio.Auth.API/TalyerStudio.Auth.API.csproj", "Services/TalyerStudio.Auth/TalyerStudio.Auth.API/"]
COPY ["Services/TalyerStudio.Auth/TalyerStudio.Auth.Application/TalyerStudio.Auth.Application.csproj", "Services/TalyerStudio.Auth/TalyerStudio.Auth.Application/"]
COPY ["Services/TalyerStudio.Auth/TalyerStudio.Auth.Domain/TalyerStudio.Auth.Domain.csproj", "Services/TalyerStudio.Auth/TalyerStudio.Auth.Domain/"]
COPY ["Services/TalyerStudio.Auth/TalyerStudio.Auth.Infrastructure/TalyerStudio.Auth.Infrastructure.csproj", "Services/TalyerStudio.Auth/TalyerStudio.Auth.Infrastructure/"]

# Copy shared projects
COPY ["Shared/TalyerStudio.Shared.Contracts/TalyerStudio.Shared.Contracts.csproj", "Shared/TalyerStudio.Shared.Contracts/"]
COPY ["Shared/TalyerStudio.Shared.Events/TalyerStudio.Shared.Events.csproj", "Shared/TalyerStudio.Shared.Events/"]
COPY ["Shared/TalyerStudio.Shared.Infrastructure/TalyerStudio.Shared.Infrastructure.csproj", "Shared/TalyerStudio.Shared.Infrastructure/"]

# Restore
RUN dotnet restore "Services/TalyerStudio.Auth/TalyerStudio.Auth.API/TalyerStudio.Auth.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/Services/TalyerStudio.Auth/TalyerStudio.Auth.API"
RUN dotnet build "TalyerStudio.Auth.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TalyerStudio.Auth.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TalyerStudio.Auth.API.dll"]