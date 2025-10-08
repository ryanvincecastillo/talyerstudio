# 🚗 TalyerStudio - Auto Shop Management System

**Status:** 🚀 Active Development (Month 1 - Week 4)  
**Version:** 0.4.0 (Authentication Module Complete!)  
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

### Frontend
- **React 18** with **TypeScript**
- **Vite** (Build tool)
- **TailwindCSS** (Styling)
- **React Router** (Navigation)
- **Axios** (HTTP client)

### Infrastructure
- **Docker** & **Docker Compose**
- **Redis** (Caching - planned)
- **RabbitMQ** (Message Queue - planned)

### External Services
- **Semaphore** (SMS notifications - planned)
- **SendGrid** (Email - planned)
- **PayMongo** (Payments - planned)

---

## 📁 PROJECT STRUCTURE

```
talyerstudio/
├── src/
│   ├── Services/
│   │   ├── TalyerStudio.Customer/        ✅ COMPLETE
│   │   │   ├── Domain/
│   │   │   ├── Application/
│   │   │   ├── Infrastructure/
│   │   │   └── API/
│   │   ├── TalyerStudio.Vehicle/         ✅ COMPLETE
│   │   ├── TalyerStudio.JobOrder/        ✅ COMPLETE
│   │   ├── TalyerStudio.Inventory/       ✅ COMPLETE
│   │   ├── TalyerStudio.Invoice/         ✅ COMPLETE
│   │   └── TalyerStudio.Auth/            ✅ COMPLETE (NEW!)
│   ├── Dashboard/                         🟡 30% COMPLETE
│   │   └── talyerstudio-dashboard/
│   └── TalyerStudio.sln
├── docker/
│   └── docker-compose.yml
├── docs/
│   └── index.html (Landing page)
└── README.md
```

---

## 🎯 FEATURE STATUS

### ✅ COMPLETED FEATURES (Week 1-4)

#### Customer Management Service ✅
- [x] Customer entity (CRUD operations)
- [x] Service Category entity (CRUD operations)
- [x] Service entity (CRUD operations)
- [x] Search and filter functionality
- [x] Soft delete support
- [x] Database: `talyerstudio_customers`
- [x] Running on port **5146**

#### Vehicle Management Service ✅
- [x] Vehicle entity with AUTO/MOTORCYCLE support
- [x] Motorcycle-specific fields (displacement, tire sizes)
- [x] Philippines-specific OR/CR expiry tracking
- [x] Full CRUD API with filters
- [x] Database: `talyerstudio_vehicles`
- [x] Running on port **5167**

#### Service Catalog ✅
- [x] Service categories with icons
- [x] Service applicability (AUTO, MOTORCYCLE, BOTH)
- [x] Display order support
- [x] Filter by category and type
- [x] Integrated with Customer Service

#### Job Order Service ✅
- [x] JobOrder, JobOrderItem, JobOrderPart entities
- [x] Status workflow (PENDING → IN_PROGRESS → COMPLETED → INVOICED)
- [x] Auto-generated job order numbers (JO-YYYYMMDD-####)
- [x] Full REST API with 8 endpoints
- [x] Database: `talyerstudio_joborders`
- [x] Running on port **5200**

#### Inventory Service ✅
- [x] Product, ProductCategory, StockLevel, StockMovement entities
- [x] Product types: PART, ACCESSORY, CHEMICAL, TIRE, BATTERY, etc.
- [x] Multi-branch stock tracking
- [x] Stock movement audit trail
- [x] Low stock alert system
- [x] SKU uniqueness validation
- [x] Database: `talyerstudio_inventory`
- [x] Running on port **5210**

#### Invoice & Payment Service ✅
- [x] Invoice entity with customer/job order linking
- [x] Invoice items with product details
- [x] Payment recording with multiple payment methods
- [x] Invoice status tracking (DRAFT, SENT, PAID, OVERDUE, CANCELLED)
- [x] Auto-generated invoice numbers (INV-YYYYMMDD-####)
- [x] Due date calculation
- [x] Database: `talyerstudio_invoices`
- [x] Running on port **5220**

#### Authentication & Authorization Service ✅ **NEW!**
- [x] User, Role, Permission entities
- [x] JWT authentication with refresh tokens
- [x] Password hashing with BCrypt
- [x] Login/Register endpoints
- [x] Token refresh endpoint
- [x] Role-based access control (RBAC)
- [x] Account lockout after failed attempts
- [x] Multi-tenancy support
- [x] Default roles seeding (Admin, User)
- [x] Permission-based authorization
- [x] Database: `talyerstudio_auth`
- [x] Running on port **5230**

**Status:** ✅ 100% Complete

---

## 🔐 AUTHENTICATION DETAILS

### Available Endpoints

**Base URL:** `http://localhost:5230/api/Auth`

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/register` | Register new user | No |
| POST | `/login` | User login | No |
| POST | `/refresh` | Refresh access token | No |
| POST | `/revoke` | Revoke refresh token | No |
| GET | `/me` | Get current user info | Yes |

### Default Test Credentials

**Tenant ID:** `00000000-0000-0000-0000-000000000001`

You can register your own user or use any registered credentials.

### Token Configuration

- **Access Token Expiry:** 60 minutes
- **Refresh Token Expiry:** 7 days
- **Algorithm:** HS256
- **Account Lockout:** After 5 failed attempts (30 min lockout)

### Seeded Permissions

The system comes with pre-configured permissions for:
- Customers (view, create, edit, delete)
- Vehicles (view, create, edit, delete)
- Job Orders (view, create, edit, delete)
- Inventory (view, create, edit, delete)
- Invoices (view, create, edit, delete)
- Users (view, create, edit, delete)

### Default Roles

- **Admin:** Full access to all permissions
- **User:** View-only access to all modules

---

## 📅 CHANGELOG

### **October 8, 2025 - Authentication Module Complete! 🎉**
- ✅ **Authentication Service COMPLETE**
  - Created User, Role, Permission, RefreshToken entities
  - Implemented JWT authentication with refresh tokens
  - Added password hashing with BCrypt (cost factor 12)
  - Implemented login/register endpoints
  - Added token refresh and revoke endpoints
  - Role-based access control (RBAC)
  - Permission-based authorization
  - Account lockout after 5 failed attempts
  - Multi-tenancy support
  - Repository pattern with clean architecture
  - Database: talyerstudio_auth
  - Running on port 5230
  - All endpoints tested and working ✓
  - Seeded default roles and permissions

### **October 8, 2025 - Invoice & Payment Module Complete**
- ✅ **Invoice Service COMPLETE**
  - Created Invoice, InvoiceItem, Payment entities
  - Multiple payment methods (CASH, CARD, BANK_TRANSFER, GCASH, MAYA, CHECK)
  - Invoice status tracking (DRAFT, SENT, PAID, OVERDUE, CANCELLED)
  - Auto-generated invoice numbers
  - Full REST API with 11 endpoints
  - Repository pattern with clean architecture
  - Database: talyerstudio_invoices
  - Running on port 5220
  - All tests passed ✓

**October 8, 2025 - Inventory Module Complete:**
- ✅ Created Product, ProductCategory, StockLevel, StockMovement entities
- ✅ Multi-branch stock tracking
- ✅ Stock movement audit trail
- ✅ Low stock alert system
- ✅ Full REST API with 13 endpoints
- ✅ Running on port 5210

**October 8, 2025 - JobOrder Module Complete:**
- ✅ Created JobOrder entities with status workflow
- ✅ Auto-generated job order numbers
- ✅ Full REST API with 8 endpoints
- ✅ Running on port 5200

**October 8, 2025 - Service Catalog Complete:**
- ✅ Created Service and ServiceCategory entities
- ✅ Support for AUTO, MOTORCYCLE, and BOTH service types
- ✅ Full CRUD APIs

**October 8, 2025 - Vehicle Module Complete:**
- ✅ Created Vehicle entity with motorcycle support
- ✅ Philippines-specific OR/CR tracking
- ✅ Full CRUD API

**October 8, 2025 - Customer Module Complete:**
- ✅ Added UPDATE and DELETE endpoints
- ✅ Added search/filter functionality

**October 7, 2025:**
- ✅ Created initial database migration
- ✅ Implemented Customer REST API

**October 6, 2025:**
- ✅ Initialized project structure
- ✅ Setup React dashboard

### Known Issues
- [ ] Dashboard needs authentication integration
- [ ] No error handling middleware
- [ ] Dashboard has no loading states
- [ ] Docker Compose not updated with Auth database

### Technical Debt
- [ ] Add global exception handling
- [ ] Add request/response logging
- [ ] Add input validation with FluentValidation
- [ ] Add API versioning
- [ ] Add health check endpoints
- [ ] Improve Swagger documentation
- [ ] Update Docker Compose with all services
- [ ] Secure all existing service endpoints with JWT

---

## 🎯 FOCUS AREAS THIS WEEK

### Week 4 Goals (October 9-15, 2025)

**Primary Goals:**
1. ✅ **Invoice & Payment Module (COMPLETE!)**
2. ✅ **Authentication & Authorization Module (COMPLETE!)**
3. ⏳ **Secure All Existing Endpoints**
   - [ ] Add JWT authentication to Customer Service
   - [ ] Add JWT authentication to Vehicle Service
   - [ ] Add JWT authentication to JobOrder Service
   - [ ] Add JWT authentication to Inventory Service
   - [ ] Add JWT authentication to Invoice Service
   
4. ⏳ **Dashboard Authentication Integration**
   - [ ] Create login page
   - [ ] Implement auth context/provider
   - [ ] Add protected routes
   - [ ] Store tokens in localStorage
   - [ ] Add token refresh logic
   - [ ] Add logout functionality

5. ⏳ **Update Docker Compose**
   - [ ] Add Auth database
   - [ ] Add all service databases
   - [ ] Test all services with Docker

**Stretch Goals:**
- Create dashboard pages for new modules
- Add role-based UI rendering
- Setup API error handling middleware

---

## 📈 PROGRESS TRACKER

### Overall MVP Progress: 80%

```
Month 1 Foundation:          ████████████████░░░░ 80%
Month 2 Enhanced Features:   ░░░░░░░░░░░░░░░░░░░░  0%
Month 3 Polish & Launch:     ░░░░░░░░░░░░░░░░░░░░  0%
```

### Feature Completion Status

| Feature                    | Status | Progress |
|---------------------------|--------|----------|
| Database Setup            | ✅     | 100%     |
| Docker Infrastructure     | ✅     | 100%     |
| Customer Management       | ✅     | 100%     |
| Vehicle Management        | ✅     | 100%     |
| Service Catalog           | ✅     | 100%     |
| Job Orders                | ✅     | 100%     |
| Inventory                 | ✅     | 100%     |
| Invoicing                 | ✅     | 100%     |
| **Authentication**        | ✅     | **100%** |
| Secure Endpoints          | ⏳     | 0%       |
| Dashboard UI              | 🟡     | 30%      |
| Dashboard Auth            | ⏳     | 0%       |
| POS System                | ⏳     | 0%       |
| Appointments              | ⏳     | 0%       |
| SMS Notifications         | ⏳     | 0%       |
| Reports                   | ⏳     | 0%       |

**Legend:**
- ✅ Complete (100%)
- 🟡 In Progress (1-99%)
- ⏳ Not Started (0%)

---

## 🎯 MILESTONE CHECKLIST

### MVP Launch Checklist (End of Month 3)

#### Core Features (Must Have)
- [x] Customer Management (CRUD)
- [x] Vehicle Registry (CRUD)
- [x] Service Catalog
- [x] Job Order Creation & Tracking
- [x] Basic Inventory Management
- [x] Invoice Generation
- [x] Payment Recording
- [x] **Authentication & Authorization** ✅
- [ ] Secure all API endpoints
- [ ] Dashboard (all core pages)
- [ ] Dashboard authentication
- [ ] Multi-tenancy (basic)

#### Essential Features (Should Have)
- [ ] POS/Cashier Interface
- [ ] Appointment Scheduling
- [ ] SMS Notifications (via Semaphore)
- [ ] Basic Reports (sales, services)
- [ ] Landing Page (deployed)

#### Nice to Have (Can Wait)
- Advanced analytics
- Mobile apps
- White-label features
- Advanced reporting

---

## 🚀 QUICK START COMMANDS

```bash
# Start all services (Docker)
cd docker
docker-compose up -d

# Run Customer Service
cd src/Services/TalyerStudio.Customer/TalyerStudio.Customer.API
dotnet run
# API: http://localhost:5180/swagger

# Run Vehicle Service
cd src/Services/TalyerStudio.Vehicle/TalyerStudio.Vehicle.API
dotnet run
# API: http://localhost:5190/swagger

# Run JobOrder Service
cd src/Services/TalyerStudio.JobOrder/TalyerStudio.JobOrder.API
dotnet run
# API: http://localhost:5200/swagger

# Run Inventory Service
cd src/Services/TalyerStudio.Inventory/TalyerStudio.Inventory.API
dotnet run
# API: http://localhost:5210/swagger

# Run Invoice Service
cd src/Services/TalyerStudio.Invoice/TalyerStudio.Invoice.API
dotnet run
# API: http://localhost:5220/swagger

# Run Auth Service (NEW!)
cd src/Services/TalyerStudio.Auth/TalyerStudio.Auth.API
dotnet run
# API: http://localhost:5230/swagger

# Run Dashboard
cd src/Dashboard/talyerstudio-dashboard
npm install
npm run dev
# Dashboard: http://localhost:5173
```

---

## 📚 RESOURCES & REFERENCES

### Documentation
- [Architecture Document](./FULL%20SYSTEM%20ARCHITECTURE.MD)
- [Landing Page](./docs/index.html)
- [gRPC Protos](./src/Shared/TalyerStudio.Shared.Infrastructure/Protos/)

### API Endpoints

**Authentication Service (Port 5230):** ✨ NEW!
- POST /api/auth/register
- POST /api/auth/login
- POST /api/auth/refresh
- POST /api/auth/revoke
- GET /api/auth/me (Protected)

**Customer Service (Port 5146):**
- GET/POST/PUT/DELETE /api/customers
- GET/POST/PUT/DELETE /api/servicecategories
- GET/POST/PUT/DELETE /api/services

**Vehicle Service (Port 5167):**
- GET/POST/PUT/DELETE /api/vehicles
- GET /api/vehicles/customer/{customerId}

**JobOrder Service (Port 5200):**
- GET/POST /api/job-orders
- GET /api/job-orders/{id}
- PATCH /api/job-orders/{id}/status
- POST /api/job-orders/{id}/assign
- DELETE /api/job-orders/{id}
- GET /api/job-orders/customer/{customerId}
- GET /api/job-orders/vehicle/{vehicleId}

**Inventory Service (Port 5210):**
- GET/POST/PUT/DELETE /api/products
- GET /api/products/low-stock
- GET/POST/PUT/DELETE /api/product-categories
- GET /api/stock/product/{productId}
- POST /api/stock/product/{productId}/adjust

**Invoice Service (Port 5220):**
- GET/POST/DELETE /api/invoices
- GET /api/invoices/{id}
- GET /api/invoices/number/{invoiceNumber}
- GET /api/invoices/customer/{customerId}
- GET /api/invoices/job-order/{jobOrderId}
- POST /api/payments
- GET /api/payments/invoice/{invoiceId}

### Databases

**Databases:**
- `talyerstudio_customers` - Customer Service database
- `talyerstudio_vehicles` - Vehicle Service database
- `talyerstudio_joborders` - JobOrder Service database
- `talyerstudio_inventory` - Inventory Service database
- `talyerstudio_invoices` - Invoice Service database
- `talyerstudio_auth` - Authentication Service database ✨ NEW!

### External Links
- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [React Documentation](https://react.dev/)
- [TailwindCSS](https://tailwindcss.com/)
- [PostgreSQL](https://www.postgresql.org/)

---

## 👥 TEAM

**Developer:** Ryan Vince Castillo  
**Designer:** TBD  
**QA:** TBD

---

## 💪 MOTIVATION

> "Building TalyerStudio is not just about creating software—it's about solving real problems for hardworking Filipino shop owners."

**Remember:**
- Start Small, Think Big
- Build it right the first time
- Focus on customer value
- Ship features, gather feedback, improve

**Week 4 Achievement:** 🎉🎉🎉
**Authentication & Authorization module completed with JWT, refresh tokens, RBAC, and clean architecture!**

---

## 📝 NOTES

- All services are using Clean Architecture pattern
- PostgreSQL is the primary database
- JWT tokens expire after 60 minutes
- Refresh tokens expire after 7 days
- Account lockout occurs after 5 failed login attempts
- All passwords are hashed using BCrypt with cost factor 12
- Multi-tenancy is implemented at the database level
- Default tenant ID for testing: `00000000-0000-0000-0000-000000000001`

---

**Last Updated:** October 8, 2025  
**Next Review:** October 15, 2025