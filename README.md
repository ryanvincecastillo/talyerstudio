# 🚗 TalyerStudio - Auto Shop Management System

**Status:** 🚀 Active Development (Month 1 - Week 5)  
**Version:** 0.8.0 (Job Orders Complete!)  
**Target Launch:** December 2025

---

## 📊 PROJECT OVERVIEW

TalyerStudio is a comprehensive, cloud-based management system designed specifically for Filipino auto repair shops (talyer) and motorcycle shops. Built with modern technologies, it streamlines operations from customer management to invoicing, inventory, and appointments.

### 🎯 Mission
Empower small to medium-sized auto shops in the Philippines with affordable, easy-to-use software that helps them manage their business efficiently and grow sustainably.

### 🌟 Core Value Proposition
- **Affordable**: Subscription-based pricing starting at ₱999/month
- **Easy to Use**: Intuitive interface designed for non-technical users
- **Comprehensive**: All-in-one solution (no need for multiple tools)
- **Philippines-Focused**: Built with Filipino shop owners in mind

---

## 🏗️ TECH STACK

### Backend
- **.NET 8** (Web API)
- **PostgreSQL** (Database)
- **Entity Framework Core** (ORM)
- **Clean Architecture** (Domain-Driven Design)
- **JWT Authentication** with Refresh Tokens
- **BCrypt** for password hashing
- **gRPC** for inter-service communication

### Frontend
- **React 18** with **TypeScript**
- **Vite** (Build tool)
- **TailwindCSS** (Styling)
- **React Router** (Navigation)
- **Axios** (HTTP client)
- **JWT** Token Management

### Infrastructure
- **Docker** & **Docker Compose**
- **Redis** (Caching - ready)
- **RabbitMQ** (Message Queue - ready)

### External Services (Planned)
- **Semaphore** (SMS notifications)
- **SendGrid** (Email)
- **PayMongo** (Payments)

---

## 📁 PROJECT STRUCTURE

```
talyerstudio/
├── src/
│   ├── Services/
│   │   ├── TalyerStudio.Customer/        ✅ COMPLETE (HTTP + gRPC)
│   │   │   ├── Domain/
│   │   │   ├── Application/
│   │   │   ├── Infrastructure/
│   │   │   └── API/
│   │   ├── TalyerStudio.Vehicle/         ✅ COMPLETE (HTTP + gRPC)
│   │   ├── TalyerStudio.JobOrder/        ✅ COMPLETE (HTTP + gRPC)
│   │   ├── TalyerStudio.Inventory/       ✅ COMPLETE (HTTP + gRPC)
│   │   ├── TalyerStudio.Invoice/         ✅ COMPLETE (HTTP + gRPC)
│   │   └── TalyerStudio.Auth/            ✅ COMPLETE (HTTP + gRPC)
│   ├── Shared/
│   │   ├── TalyerStudio.Shared.Contracts/
│   │   ├── TalyerStudio.Shared.Events/
│   │   ├── TalyerStudio.Shared.Infrastructure/
│   │   └── TalyerStudio.Shared.Protos/   ✅ gRPC Proto Files
│   ├── Clients/
│   │   └── talyerstudio-dashboard/       🟢 85% COMPLETE
│   └── TalyerStudio.sln
├── docker/
│   └── docker-compose.yml                ✅ ALL SERVICES
├── docs/
│   └── index.html (Landing page)
└── README.md
```

---

## 🎯 FEATURE STATUS

### ✅ COMPLETED FEATURES (Weeks 1-5)

#### 🔐 Authentication & Authorization System ✅
- [x] JWT authentication with refresh tokens
- [x] Password hashing with BCrypt (cost factor 12)
- [x] Login/Register endpoints
- [x] Token refresh and revoke endpoints
- [x] Role-based access control (RBAC)
- [x] Permission-based authorization
- [x] Account lockout after 5 failed attempts
- [x] Multi-tenancy support
- [x] Database: `talyerstudio_auth`
- [x] Running on port **5230** (HTTP) + **5232** (gRPC)

#### 👥 Customer Management Service ✅
- [x] Customer entity (CRUD operations)
- [x] Service Category entity (CRUD operations)
- [x] Service entity (CRUD operations with categories)
- [x] Search and filter functionality
- [x] Soft delete support
- [x] Individual/Corporate customer types
- [x] Philippine address support
- [x] Loyalty points system
- [x] Database: `talyerstudio_customers`
- [x] Running on port **5180** (HTTP) + **5182** (gRPC)

#### 🚗 Vehicle Management Service ✅
- [x] Vehicle entity with AUTO/MOTORCYCLE support
- [x] Motorcycle-specific fields (displacement, tire sizes)
- [x] Philippines-specific OR/CR expiry tracking
- [x] Full CRUD API with filters
- [x] Customer-vehicle relationship
- [x] Database: `talyerstudio_vehicles`
- [x] Running on port **5190** (HTTP) + **5192** (gRPC)

#### 🔧 Service Catalog ✅
- [x] Service categories with icons
- [x] Service applicability (AUTO, MOTORCYCLE, BOTH)
- [x] Display order support
- [x] Filter by category and type
- [x] Price management
- [x] Duration estimates
- [x] Active/Inactive status

#### 📋 Job Order Service ✅
- [x] JobOrder, JobOrderItem, JobOrderPart entities
- [x] Status workflow (PENDING → IN_PROGRESS → COMPLETED → INVOICED → CANCELLED)
- [x] Priority levels (LOW, NORMAL, HIGH, URGENT)
- [x] Auto-generated job order numbers (JO-YYYYMMDD-####)
- [x] Multiple services per job order
- [x] Auto-calculated totals
- [x] Customer complaints & inspection notes
- [x] Odometer reading tracking
- [x] Filter by status
- [x] Full REST API with 8 endpoints
- [x] Database: `talyerstudio_joborders`
- [x] Running on port **5200** (HTTP) + **5202** (gRPC)

#### 📦 Inventory Service ✅
- [x] Product, ProductCategory, StockLevel, StockMovement entities
- [x] Product types: PART, ACCESSORY, CHEMICAL, TIRE, BATTERY, etc.
- [x] Multi-branch stock tracking
- [x] Stock movement audit trail
- [x] Low stock alert system
- [x] SKU uniqueness validation
- [x] Database: `talyerstudio_inventory`
- [x] Running on port **5210** (HTTP) + **5212** (gRPC)

#### 🧾 Invoice & Payment Service ✅
- [x] Invoice entity with customer/job order linking
- [x] Invoice items with product details
- [x] Payment recording with multiple payment methods
- [x] Invoice status tracking (DRAFT, SENT, PAID, OVERDUE, CANCELLED)
- [x] Auto-generated invoice numbers (INV-YYYYMMDD-####)
- [x] Due date calculation
- [x] Database: `talyerstudio_invoices`
- [x] Running on port **5220** (HTTP) + **5222** (gRPC)

#### 🖥️ Dashboard (React + TypeScript) 🟢
- [x] Login & Register pages
- [x] Protected routes with JWT
- [x] Token auto-refresh
- [x] Dashboard layout with sidebar
- [x] Customers page (Full CRUD)
- [x] Vehicles page (Full CRUD with motorcycle support)
- [x] Services page (Categories + Services management)
- [x] Job Orders page (Full workflow)
- [ ] Inventory pages
- [ ] Invoices page
- [ ] Dashboard home with statistics
- [ ] POS/Cashier interface

---

## 🚀 QUICK START COMMANDS

### Start All Services (Docker)

```bash
# Navigate to docker directory
cd docker

# Start all services
docker compose up -d

# Check running containers
docker ps

# View logs
docker compose logs -f

# Stop all services
docker compose down
```

### Run Individual Services (Development)

```bash
# Customer Service
cd src/Services/TalyerStudio.Customer/TalyerStudio.Customer.API
dotnet run
# API: http://localhost:5180/swagger
# gRPC: http://localhost:5182

# Vehicle Service
cd src/Services/TalyerStudio.Vehicle/TalyerStudio.Vehicle.API
dotnet run
# API: http://localhost:5190/swagger
# gRPC: http://localhost:5192

# JobOrder Service
cd src/Services/TalyerStudio.JobOrder/TalyerStudio.JobOrder.API
dotnet run
# API: http://localhost:5200/swagger
# gRPC: http://localhost:5202

# Inventory Service
cd src/Services/TalyerStudio.Inventory/TalyerStudio.Inventory.API
dotnet run
# API: http://localhost:5210/swagger
# gRPC: http://localhost:5212

# Invoice Service
cd src/Services/TalyerStudio.Invoice/TalyerStudio.Invoice.API
dotnet run
# API: http://localhost:5220/swagger
# gRPC: http://localhost:5222

# Auth Service
cd src/Services/TalyerStudio.Auth/TalyerStudio.Auth.API
dotnet run
# API: http://localhost:5230/swagger
# gRPC: http://localhost:5232

# Dashboard
cd src/Clients/talyerstudio-dashboard
npm install
npm run dev
# Dashboard: http://localhost:5173
```

---

## 📚 API ENDPOINTS

### Authentication Service (Port 5230)
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh` - Refresh access token
- `POST /api/auth/revoke` - Revoke refresh token
- `GET /api/auth/me` - Get current user (requires auth)

### Customer Service (Port 5180)
- `GET /api/customers` - Get all customers
- `POST /api/customers` - Create customer
- `GET /api/customers/{id}` - Get customer by ID
- `PUT /api/customers/{id}` - Update customer
- `DELETE /api/customers/{id}` - Delete customer

### Service Catalog (Port 5180)
- `GET /api/servicecategories` - Get all categories
- `POST /api/servicecategories` - Create category
- `GET /api/services` - Get all services
- `POST /api/services` - Create service
- `PUT /api/services/{id}` - Update service

### Vehicle Service (Port 5190)
- `GET /api/vehicles` - Get all vehicles
- `POST /api/vehicles` - Create vehicle
- `GET /api/vehicles/{id}` - Get vehicle by ID
- `GET /api/vehicles/customer/{customerId}` - Get vehicles by customer
- `PUT /api/vehicles/{id}` - Update vehicle
- `DELETE /api/vehicles/{id}` - Delete vehicle

### Job Order Service (Port 5200)
- `GET /api/job-orders` - Get all job orders
- `POST /api/job-orders` - Create job order
- `GET /api/job-orders/{id}` - Get job order by ID
- `PUT /api/job-orders/{id}` - Update job order
- `PATCH /api/job-orders/{id}/status` - Update status
- `GET /api/job-orders/customer/{customerId}` - Get by customer
- `DELETE /api/job-orders/{id}` - Delete job order

### Inventory Service (Port 5210)
- `GET /api/products` - Get all products
- `POST /api/products` - Create product
- `GET /api/stock-levels` - Get stock levels
- `POST /api/stock-movements` - Record stock movement

### Invoice Service (Port 5220)
- `GET /api/invoices` - Get all invoices
- `POST /api/invoices` - Create invoice
- `GET /api/invoices/{id}` - Get invoice by ID
- `POST /api/invoices/{id}/payments` - Record payment

---

## 🔐 AUTHENTICATION

All endpoints (except auth endpoints) require JWT authentication.

**Get a token:**
```bash
curl -X POST http://localhost:5230/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "your@email.com",
    "password": "yourpassword",
    "tenantId": "00000000-0000-0000-0000-000000000001"
  }'
```

**Use the token:**
```bash
curl -X GET http://localhost:5180/api/customers \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

---

## 📈 PROGRESS TRACKER

### Overall MVP Progress: 85%

```
Month 1 Foundation:          ████████████████░ 85%
Month 2 Enhanced Features:   ░░░░░░░░░░░░░░░░  0%
Month 3 Polish & Launch:     ░░░░░░░░░░░░░░░░  0%
```

### Feature Completion Status

| Feature                    | Backend | Frontend | Progress |
|---------------------------|---------|----------|----------|
| Database Setup            | ✅     | N/A      | 100%     |
| Docker Infrastructure     | ✅     | N/A      | 100%     |
| Customer Management       | ✅     | ✅       | 100%     |
| Vehicle Management        | ✅     | ✅       | 100%     |
| Service Catalog           | ✅     | ✅       | 100%     |
| Job Orders                | ✅     | ✅       | 100%     |
| Inventory                 | ✅     | ⏳       | 50%      |
| Invoicing                 | ✅     | ⏳       | 50%      |
| Authentication            | ✅     | ✅       | 100%     |
| Dashboard UI              | N/A    | 🟢       | 85%      |
| POS System                | ⏳     | ⏳       | 0%       |
| Appointments              | ⏳     | ⏳       | 0%       |
| SMS Notifications         | ⏳     | ⏳       | 0%       |
| Reports                   | ⏳     | ⏳       | 0%       |

**Legend:**
- ✅ Complete (100%)
- 🟢 In Progress (75-99%)
- 🟡 In Progress (1-74%)
- ⏳ Not Started (0%)

---

## 📅 CHANGELOG

### **November 11, 2025 - Job Orders Complete!** 🎉
- ✅ **Job Orders Full CRUD**
  - Create job orders with multiple services
  - Customer and vehicle selection
  - Priority levels (LOW, NORMAL, HIGH, URGENT)
  - Status workflow tracking
  - Auto-calculated totals
  - Filter by status
  - Odometer tracking
  - Customer complaints & inspection notes
  - Full dashboard integration

### **November 10, 2025 - Services Management Complete**
- ✅ **Service Catalog Full CRUD**
  - Service categories with emojis
  - Services with pricing and duration
  - Applicability filtering (Auto/Motorcycle/Both)
  - Active/Inactive status management
  - Dashboard pages with tabs

### **November 9, 2025 - Dashboard Auth Integration Complete**
- ✅ **Dashboard Authentication**
  - Login/Register pages
  - Protected routes
  - Token storage and auto-refresh
  - Logout functionality
  - Auth context provider

### **November 8, 2025 - Authentication Module Complete**
- ✅ Created User, Role, Permission, RefreshToken entities
- ✅ Implemented JWT authentication with refresh tokens
- ✅ Added password hashing with BCrypt
- ✅ Role-based access control (RBAC)
- ✅ Seeded default roles and permissions

### **October 8, 2025 - Invoice & Payment Module Complete**
- ✅ Created Invoice, InvoiceItem, Payment entities
- ✅ Multiple payment methods support
- ✅ Invoice status tracking
- ✅ Auto-generated invoice numbers

### **October 8, 2025 - Inventory Module Complete**
- ✅ Created Product, ProductCategory, StockLevel, StockMovement entities
- ✅ Multi-branch stock tracking
- ✅ Stock movement audit trail

### **October 8, 2025 - JobOrder Module Complete**
- ✅ Created JobOrder entities with status workflow
- ✅ Auto-generated job order numbers

### **October 8, 2025 - Service Catalog Complete**
- ✅ Created Service and ServiceCategory entities
- ✅ Support for AUTO, MOTORCYCLE, and BOTH service types

### **October 8, 2025 - Vehicle Module Complete**
- ✅ Created Vehicle entity with motorcycle support
- ✅ Philippines-specific OR/CR tracking

### **October 7-8, 2025 - Customer Module Complete**
- ✅ Full CRUD operations
- ✅ Search/filter functionality

---

## 🎯 NEXT STEPS

### Immediate (This Week)
1. ⏳ Build Inventory management dashboard pages
2. ⏳ Build Invoice management dashboard pages
3. ⏳ Connect Invoices to Job Orders
4. ⏳ Add real statistics to Dashboard home

### Short Term (Next 2 Weeks)
1. ⏳ POS/Cashier interface
2. ⏳ Appointment scheduling
3. ⏳ SMS notifications (Semaphore integration)
4. ⏳ Basic reporting

### Mid Term (Month 2)
1. ⏳ Advanced inventory (purchase orders, stock transfers)
2. ⏳ Employee/Staff management
3. ⏳ Role-based permissions refinement
4. ⏳ Enhanced reporting with charts

---

## 🐛 KNOWN ISSUES

- [ ] Dashboard home shows placeholder statistics (needs real data)
- [ ] No global error handling middleware
- [ ] No request/response logging
- [ ] Dashboard has no loading states in some pages

---

## 🔧 TECHNICAL DEBT

- [ ] Add global exception handling
- [ ] Add request/response logging
- [ ] Add input validation with FluentValidation
- [ ] Add API versioning
- [ ] Add health check endpoints
- [ ] Improve Swagger documentation
- [ ] Add unit tests
- [ ] Add integration tests

---

## 📖 DOCUMENTATION

- [Full System Architecture](./FULL%20SYSTEM%20ARCHITECTURE.MD)
- [Landing Page](./docs/index.html)
- [gRPC Proto Files](./src/Shared/TalyerStudio.Shared.Protos/)

---

## 🤝 CONTRIBUTING

This is a solo project currently under active development. Once the MVP is complete, contribution guidelines will be added.

---

## 📄 LICENSE

Proprietary - All rights reserved

---

## 👨‍💻 DEVELOPER

**Ryan Vince Castillo**
- Building TalyerStudio from scratch
- Focus: Full-stack development (.NET + React)
- Goal: Launch a successful SaaS for Philippine auto shops

---

## 🎊 MILESTONES

- ✅ Week 1: Database & Infrastructure Setup
- ✅ Week 2: Customer & Vehicle Modules
- ✅ Week 3: Service Catalog & Job Orders Backend
- ✅ Week 4: Authentication & Authorization
- ✅ Week 5: Dashboard Integration & Job Orders Complete
- ⏳ Week 6: Inventory & Invoices Dashboard
- ⏳ Week 8: POS & Appointments
- ⏳ Week 12: MVP Launch

---

**Last Updated:** November 11, 2025  
**Current Focus:** Inventory & Invoice Dashboard Pages  
**Next Milestone:** Complete Core CRUD Operations (Week 6)