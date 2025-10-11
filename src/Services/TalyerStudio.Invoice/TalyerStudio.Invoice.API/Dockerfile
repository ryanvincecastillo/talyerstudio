FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["TalyerStudio.sln", "./"]

# Copy all csproj files for Invoice Service
COPY ["Services/TalyerStudio.Invoice/TalyerStudio.Invoice.API/TalyerStudio.Invoice.API.csproj", "Services/TalyerStudio.Invoice/TalyerStudio.Invoice.API/"]
COPY ["Services/TalyerStudio.Invoice/TalyerStudio.Invoice.Application/TalyerStudio.Invoice.Application.csproj", "Services/TalyerStudio.Invoice/TalyerStudio.Invoice.Application/"]
COPY ["Services/TalyerStudio.Invoice/TalyerStudio.Invoice.Domain/TalyerStudio.Invoice.Domain.csproj", "Services/TalyerStudio.Invoice/TalyerStudio.Invoice.Domain/"]
COPY ["Services/TalyerStudio.Invoice/TalyerStudio.Invoice.Infrastructure/TalyerStudio.Invoice.Infrastructure.csproj", "Services/TalyerStudio.Invoice/TalyerStudio.Invoice.Infrastructure/"]

# Copy shared projects
COPY ["Shared/TalyerStudio.Shared.Contracts/TalyerStudio.Shared.Contracts.csproj", "Shared/TalyerStudio.Shared.Contracts/"]
COPY ["Shared/TalyerStudio.Shared.Events/TalyerStudio.Shared.Events.csproj", "Shared/TalyerStudio.Shared.Events/"]
COPY ["Shared/TalyerStudio.Shared.Infrastructure/TalyerStudio.Shared.Infrastructure.csproj", "Shared/TalyerStudio.Shared.Infrastructure/"]

# Restore
RUN dotnet restore "Services/TalyerStudio.Invoice/TalyerStudio.Invoice.API/TalyerStudio.Invoice.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/Services/TalyerStudio.Invoice/TalyerStudio.Invoice.API"
RUN dotnet build "TalyerStudio.Invoice.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TalyerStudio.Invoice.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TalyerStudio.Invoice.API.dll"]