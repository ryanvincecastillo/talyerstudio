FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["TalyerStudio.sln", "./"]

# Copy all csproj files for JobOrder Service
COPY ["Services/TalyerStudio.JobOrder/TalyerStudio.JobOrder.API/TalyerStudio.JobOrder.API.csproj", "Services/TalyerStudio.JobOrder/TalyerStudio.JobOrder.API/"]
COPY ["Services/TalyerStudio.JobOrder/TalyerStudio.JobOrder.Application/TalyerStudio.JobOrder.Application.csproj", "Services/TalyerStudio.JobOrder/TalyerStudio.JobOrder.Application/"]
COPY ["Services/TalyerStudio.JobOrder/TalyerStudio.JobOrder.Domain/TalyerStudio.JobOrder.Domain.csproj", "Services/TalyerStudio.JobOrder/TalyerStudio.JobOrder.Domain/"]
COPY ["Services/TalyerStudio.JobOrder/TalyerStudio.JobOrder.Infrastructure/TalyerStudio.JobOrder.Infrastructure.csproj", "Services/TalyerStudio.JobOrder/TalyerStudio.JobOrder.Infrastructure/"]

# Copy shared projects
COPY ["Shared/TalyerStudio.Shared.Contracts/TalyerStudio.Shared.Contracts.csproj", "Shared/TalyerStudio.Shared.Contracts/"]
COPY ["Shared/TalyerStudio.Shared.Events/TalyerStudio.Shared.Events.csproj", "Shared/TalyerStudio.Shared.Events/"]
COPY ["Shared/TalyerStudio.Shared.Infrastructure/TalyerStudio.Shared.Infrastructure.csproj", "Shared/TalyerStudio.Shared.Infrastructure/"]

# Restore
RUN dotnet restore "Services/TalyerStudio.JobOrder/TalyerStudio.JobOrder.API/TalyerStudio.JobOrder.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/Services/TalyerStudio.JobOrder/TalyerStudio.JobOrder.API"
RUN dotnet build "TalyerStudio.JobOrder.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TalyerStudio.JobOrder.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TalyerStudio.JobOrder.API.dll"]