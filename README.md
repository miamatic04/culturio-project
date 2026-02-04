# Span.Culturio
Culturio platform for managing cultural institutions, users, subscriptions, and visits.

## Overview
This repository contains the implementations of **Tasks 1, 2, and 3** for the course  
**Agilni razvoj digitalnih platformi s pomoću mikroservisa (ARDPSPM)**  
(Span @ FER).

The project is developed incrementally through course assignments:
- **Task 1** focuses on building a **monolithic .NET REST API**
- **Task 2** refactors the monolith into a **microservice-based architecture**
- **Task 3** adds **containerization with Docker** and an **API Gateway**

All implementations are kept in this repository for reference, comparison, and further development.

---

## Repository Structure
```
culturio-project/
├── Span.Culturio                   → Task 1 – Monolithic application
└── Span.Culturio.Microservices     → Task 2 & 3 – Microservice architecture
    ├── Span.Culturio.Auth
    ├── Span.Culturio.Users
    ├── Span.Culturio.CultureObjects
    ├── Span.Culturio.Subscriptions
    ├── Span.Culturio.Packages
    ├── Span.Culturio.ApiGateway    → Task 3 – API Gateway (Ocelot)
    ├── docker-compose.yml          → Task 3 – Docker orchestration
    └── images/                     → Task 3 – Docker screenshots
```

---

## Task 1 – Monolithic Application

### Assignment Description
The goal of Task 1 is to implement a platform in the form of a **monolithic application** that connects:
- cultural institutions offering visits, and
- users who purchase monthly subscriptions to access these institutions at reduced prices.

### Implemented Requirements
- Created a solution named **Span.Culturio**
- Created an API project **Span.Culturio.Api**
- Created a SQL database named **Span.Culturio**
- Implemented REST API methods according to the provided OpenAPI (Swagger) specification
- Connected all endpoints to the database for data persistence and retrieval
- Implemented `/auth/login` and `/auth/register` as placeholder endpoints returning `200 OK`

### API Summary (Task 1)

**Auth**
- `POST /auth/register` — Placeholder endpoint
- `POST /auth/login` — Placeholder endpoint

**Users**
- `GET /users`
- `GET /users/{id}`

**Culture Objects**
- `POST /culture-objects`
- `GET /culture-objects`
- `GET /culture-objects/{id}`

**Subscriptions**
- `POST /subscriptions`
- `GET /subscriptions`
- `POST /subscriptions/activate`
- `POST /subscriptions/track-visit`

**Packages**
- `GET /packages`

---

## Task 2 – Microservice Architecture

### Assignment Description
The goal of Task 2 is to refactor the monolithic application from Task 1 into a **microservice-based system**.
Each microservice represents a separate domain, includes cross-cutting concerns, and is connected to its own database.

### Implemented Requirements
- Created a new solution: **Span.Culturio.Microservices**
- Split the monolithic application into independent services
- Implemented:
  - **JWT-based authentication and authorization**
  - **User role validation** (User, Admin)
  - **Input validation** using Data Annotations
  - **Logging** with ASP.NET Core ILogger
- Created separate databases and connected them to the appropriate services
- Reused and reorganized the code from Task 1 into multiple projects

### Implemented Services
- **Span.Culturio.Auth** — User registration and authentication
- **Span.Culturio.Users** — User management
- **Span.Culturio.CultureObjects** — Cultural institution management
- **Span.Culturio.Subscriptions** — Subscription and visit tracking
- **Span.Culturio.Packages** — Package and bundle management

### Databases
| Service            | Database                | Tables                          |
|--------------------|-------------------------|---------------------------------|
| Auth, Users        | Culturio.Users          | Users                           |
| CultureObjects     | Culturio.CultureObjects | CultureObjects                  |
| Subscriptions      | Culturio.Subscriptions  | Subscriptions                   |
| Packages           | Culturio.Packages       | Packages, PackageCultureObjects |

### API Summary (Task 2)
The API remains aligned with the OpenAPI specification from Task 1 but is distributed across multiple services.

**Auth Service** (Port 7001)
- `POST /auth/register`
- `POST /auth/login`

**Users Service** (Port 7002)
- `GET /users`
- `GET /users/{id}`

**Culture Objects Service** (Port 7003)
- `POST /culture-objects`
- `GET /culture-objects`
- `GET /culture-objects/{id}`

**Subscriptions Service** (Port 7004)
- `POST /subscriptions`
- `GET /subscriptions`
- `POST /subscriptions/activate`
- `POST /subscriptions/track-visit`

**Packages Service** (Port 7005)
- `GET /packages`

---

## Task 3 – API Gateway and Docker

### Assignment Description
The goal of Task 3 is to **containerize all microservices** using Docker and expose them through a **unified API Gateway**.

This enables:
- Easy deployment and orchestration with Docker Compose
- Single entry point for all microservices via API Gateway
- Simplified local development and production deployments

### Implemented Requirements
- Created **Dockerfiles** for all microservices (Auth, Users, CultureObjects, Subscriptions, Packages)
- Created a new project: **Span.Culturio.ApiGateway** using **Ocelot**
- Configured routing in `ocelot.json` to expose all services via a single endpoint
- Created `docker-compose.yml` for orchestrating:
  - SQL Server database container
  - 5 microservice containers
  - 1 API Gateway container
- Verified deployment with `docker container list` command
- Captured screenshot of running containers

### Docker Setup

#### **Containers**
| Container Name            | Service              | Port Mapping    | Purpose                          |
|---------------------------|----------------------|-----------------|----------------------------------|
| culturio-sqlserver        | SQL Server 2022      | 1433:1433       | Shared database server           |
| culturio-auth             | Auth Service         | 5001:80         | Authentication & registration    |
| culturio-users            | Users Service        | 5002:80         | User management                  |
| culturio-cultureobjects   | CultureObjects       | 5003:80         | Cultural institution management  |
| culturio-subscriptions    | Subscriptions        | 5004:80         | Subscription & visit tracking    |
| culturio-packages         | Packages             | 5005:80         | Package management               |
| culturio-gateway          | API Gateway (Ocelot) | 5000:80         | Unified API entry point          |

#### **Databases**
All databases run inside the `culturio-sqlserver` container:
- `Culturio.Users`
- `Culturio.CultureObjects`
- `Culturio.Subscriptions`
- `Culturio.Packages`

### API Gateway Routes

All microservices are accessible through the API Gateway at **http://localhost:5000**:

| Route                         | Downstream Service      |
|-------------------------------|-------------------------|
| `/auth/*`                     | auth-service:80         |
| `/users/*`                    | users-service:80        |
| `/culture-objects/*`          | cultureobjects-service:80 |
| `/subscriptions/*`            | subscriptions-service:80 |
| `/packages/*`                 | packages-service:80     |

### Running the Application

#### **Prerequisites**
- Docker Desktop installed
- .NET 8.0 SDK (for local development)

#### **Start all services:**
```bash
docker-compose up -d
```

#### **Verify containers are running:**
```bash
docker container list
```

#### **Stop all services:**
```bash
docker-compose down
```

#### **Stop and remove volumes (reset database):**
```bash
docker-compose down -v
```

### API Usage Example

**Register via API Gateway:**
```bash
curl -X POST http://localhost:5000/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "username": "johndoe",
    "password": "SecurePass123!"
  }'
```

**Login via API Gateway:**
```bash
curl -X POST http://localhost:5000/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "johndoe",
    "password": "SecurePass123!"
  }'
```

**Get users (authenticated):**
```bash
curl -X GET "http://localhost:5000/users?pageSize=10&pageIndex=0" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

## Technology Stack

### Backend
- **Framework:** ASP.NET Core 8.0 (Web API)
- **Authentication:** JWT Bearer Tokens
- **ORM:** Entity Framework Core 8.0
- **Database:** SQL Server (LocalDB for dev, SQL Server 2022 for Docker)
- **API Gateway:** Ocelot 23.2.2
- **Containerization:** Docker & Docker Compose
- **Password Hashing:** BCrypt.Net

### Tools & Infrastructure
- **Version Control:** Git (Azure DevOps)
- **API Documentation:** Swagger/OpenAPI
- **Container Orchestration:** Docker Compose

---

## Security Notes

### Development
- JWT Secret Key stored in **User Secrets** (not committed to Git)
- Database passwords configured via environment variables
- HTTPS redirection enabled

### Docker Environment
- Secrets passed via environment variables in `docker-compose.yml`
- **⚠️ WARNING:** Current setup uses hardcoded passwords for demonstration purposes only
- **Production deployment requires:**
  - Docker Secrets or Azure Key Vault
  - Encrypted connection strings
  - Proper certificate management

---

## Branch Structure

| Branch   | Purpose                                  |
|----------|------------------------------------------|
| `main`   | Task 1 (Monolithic) & Task 2 (Microservices) |
| `DZ3`    | Task 3 (Docker & API Gateway)            |

---

## Project Status & Future Work

### Completed
- ✅ Task 1: Monolithic REST API
- ✅ Task 2: Microservice architecture with JWT auth
- ✅ Task 3: Docker containerization and API Gateway

### Planned
- Task 4: Kubernetes deployment

---

## Contributors
- **Mia Matić** — Developer
- **Course:** ARDPSPM (Span @ FER)

---

## License
This project is part of a university course assignment and is intended for educational purposes.

---

*Last updated: January 2026*
