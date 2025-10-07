# TalyerStudio - Auto & Motorcycle Shop Management System

Complete all-in-one management system for auto repair shops, motorcycle shops, car wash, and detailing centers in the Philippines.

## 🚗 🏍️ Features

- Customer & Vehicle Management
- Job Orders & Service Tracking
- Inventory Management
- Point of Sale (POS)
- Invoicing & Payments
- Appointment Scheduling
- Reports & Analytics
- SMS Notifications
- Multi-branch Support

## 🏗️ Architecture

Monorepo with microservices architecture:
- **Backend:** .NET 8 (C#)
- **Frontend:** React + TypeScript
- **Database:** PostgreSQL
- **Cache:** Redis
- **Message Broker:** RabbitMQ
- **Communication:** gRPC + REST API

## 📁 Project Structure
talyerstudio/
├── src/
│   ├── Services/          # Microservices
│   ├── Gateway/           # API Gateway
│   ├── Shared/            # Shared libraries
│   └── Clients/           # Frontend apps
├── tests/                 # Tests
├── docker/                # Docker configs
└── docs/                  # Documentation

## 🚀 Getting Started

Coming soon...

## 📝 License

Proprietary - All rights reserved

✅ Checklist for Step 1:

 Created talyerstudio/ folder
 Initialized Git repository
 Created folder structure (Services, Gateway, Shared, Clients, etc.)
 Created .gitignore file
 Created README.md file
 Verified structure with tree command

 ✅ Checklist for Step 2:

 Created TalyerStudio.sln solution file
 Created Customer Service with 4 projects (API, Application, Domain, Infrastructure)
 Created 3 Shared projects (Contracts, Events, Infrastructure)
 Added all projects to solution
 Setup project references (Clean Architecture)
 Built solution successfully (dotnet build)

 ✅ Checklist for Step 3:

 Installed NuGet packages (PostgreSQL, EF Core)
 Created Customer domain entity
 Created CustomerDbContext with proper mapping
 Updated appsettings.json with connection string
 Updated Program.cs to register DbContext
 Built solution successfully
 Created initial database migration

✅ Checklist for Step 4:

 Created docker-compose.yml
 Started Docker services (postgres, redis, rabbitmq)
 Ran database migration successfully
 Created CustomerDto contracts
 Created CustomersController with GET and POST endpoints
 Built and ran the API
 Tested endpoints with curl (created customer successfully)


 ✅ Checklist for Step 5:

 Installed gRPC packages
 Created customer.proto file
 Created CustomerGrpcService in Customer API
 Updated Customer API to map gRPC service
 Created Vehicle Service
 Added gRPC client to Vehicle Service
 Created test endpoints in Vehicle controller
 Both services build successfully
 Both services running
 gRPC communication working (Vehicle → Customer)

 ✅ Checklist for Step 6:

 Created Dockerfiles for Customer and Vehicle services
 Updated docker-compose.yml with all services
 Initialized React dashboard with Vite + TypeScript
 Installed Tailwind CSS
 Created basic dashboard showing customers
 Dashboard running and displaying data from API

 📌 Summary of What We've Built:
You now have a complete monorepo microservices setup:

✅ Customer Service (REST API + gRPC)
✅ Vehicle Service (calls Customer via gRPC)
✅ PostgreSQL, Redis, RabbitMQ (Docker)
✅ React Dashboard (TypeScript + Tailwind)
✅ Service-to-service communication via gRPC
✅ Clean Architecture in .NET
✅ Monorepo structure