FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["TalyerStudio.sln", "./"]

# Copy all csproj files
COPY ["Services/TalyerStudio.Customer/TalyerStudio.Customer.API/TalyerStudio.Customer.API.csproj", "Services/TalyerStudio.Customer/TalyerStudio.Customer.API/"]
COPY ["Services/TalyerStudio.Customer/TalyerStudio.Customer.Application/TalyerStudio.Customer.Application.csproj", "Services/TalyerStudio.Customer/TalyerStudio.Customer.Application/"]
COPY ["Services/TalyerStudio.Customer/TalyerStudio.Customer.Domain/TalyerStudio.Customer.Domain.csproj", "Services/TalyerStudio.Customer/TalyerStudio.Customer.Domain/"]
COPY ["Services/TalyerStudio.Customer/TalyerStudio.Customer.Infrastructure/TalyerStudio.Customer.Infrastructure.csproj", "Services/TalyerStudio.Customer/TalyerStudio.Customer.Infrastructure/"]
COPY ["Shared/TalyerStudio.Shared.Contracts/TalyerStudio.Shared.Contracts.csproj", "Shared/TalyerStudio.Shared.Contracts/"]
COPY ["Shared/TalyerStudio.Shared.Events/TalyerStudio.Shared.Events.csproj", "Shared/TalyerStudio.Shared.Events/"]
COPY ["Shared/TalyerStudio.Shared.Infrastructure/TalyerStudio.Shared.Infrastructure.csproj", "Shared/TalyerStudio.Shared.Infrastructure/"]

# Restore
RUN dotnet restore "Services/TalyerStudio.Customer/TalyerStudio.Customer.API/TalyerStudio.Customer.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/Services/TalyerStudio.Customer/TalyerStudio.Customer.API"
RUN dotnet build "TalyerStudio.Customer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TalyerStudio.Customer.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TalyerStudio.Customer.API.dll"]
