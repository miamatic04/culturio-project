# Span.Culturio
Culturio platform for managing cultural institutions, users, subscriptions, and visits.

## Overview
This repository contains the implementations of **all four tasks** for the course  
**Agilni razvoj digitalnih platformi s pomoću mikroservisa (ARDPSPM)**  
(Span @ FER).

The project is developed incrementally through course assignments:
- **Task 1** focuses on building a **monolithic .NET REST API**
- **Task 2** refactors the monolith into a **microservice-based architecture**
- **Task 3** adds **containerization with Docker** and an **API Gateway**
- **Task 4** deploys microservices to **Kubernetes** with orchestration

All implementations are kept in this repository for reference, comparison, and further development.

---

## Repository Structure
```
culturio-project/
├── Span.Culturio                   → Task 1 – Monolithic application
└── Span.Culturio.Microservices     → Tasks 2, 3 & 4 – Microservice architecture
    ├── Span.Culturio.Auth
    ├── Span.Culturio.Users
    ├── Span.Culturio.CultureObjects
    ├── Span.Culturio.Subscriptions
    ├── Span.Culturio.Packages
    ├── Span.Culturio.ApiGateway    → Task 3 – API Gateway (Ocelot)
    ├── docker-compose.yml          → Task 3 – Docker orchestration
    ├── k8s/                        → Task 4 – Kubernetes manifests
    │   ├── namespace.yaml
    │   ├── sqlserver/
    │   │   ├── deployment.yaml
    │   │   └── service.yaml
    │   ├── auth/
    │   │   ├── deployment.yaml
    │   │   └── service.yaml
    │   ├── users/
    │   │   ├── deployment.yaml
    │   │   └── service.yaml
    │   ├── cultureobjects/
    │   │   ├── deployment.yaml
    │   │   └── service.yaml
    │   ├── subscriptions/
    │   │   ├── deployment.yaml
    │   │   └── service.yaml
    │   ├── packages/
    │   │   ├── deployment.yaml
    │   │   └── service.yaml
    │   ├── apigateway/
    │   │   ├── deployment.yaml
    │   │   └── service.yaml
    │   └── ingress.yaml
    └── images/                     → Screenshots 
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

### Running the Application with Docker

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

### API Usage Example (Docker)

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

## Task 4 – Kubernetes Deployment

### Assignment Description
The goal of Task 4 is to **deploy all microservices to Kubernetes** with proper orchestration, service discovery, and ingress configuration.

This enables:
- Production-ready container orchestration
- Service discovery and load balancing
- Horizontal scaling capabilities
- Health monitoring and self-healing

### Implemented Requirements
- Created **Deployment manifests** for all microservices (deployment.yaml)
- Created **Service manifests** for all microservices (service.yaml)
- Created **Ingress manifest** with HTTP routing rules for API Gateway
- Installed **ingress-nginx controller** in Minikube
- Deployed all microservices to Minikube
- Verified deployment with kubectl commands
- Captured screenshots of:
  - All services (`kubectl get services`)
  - All deployments (`kubectl get deployments`)
  - Logs from Users service (`kubectl logs`)

### Kubernetes Setup

#### **Cluster Information**
- **Platform:** Minikube
- **Version:** v1.38.0
- **Kubernetes Version:** v1.28.3
- **Driver:** Docker (or Hyper-V/VirtualBox)
- **Namespace:** `culturio`
- **Ingress Controller:** nginx-ingress

#### **Deployed Resources**

| Resource Type | Name                     | Replicas | Image                          | Port |
|---------------|--------------------------|----------|--------------------------------|------|
| Deployment    | sqlserver                | 1        | mssql/server:2022-latest       | 1433 |
| Deployment    | auth-service             | 2        | culturio-auth:latest           | 80   |
| Deployment    | users-service            | 2        | culturio-users:latest          | 80   |
| Deployment    | cultureobjects-service   | 2        | culturio-cultureobjects:latest | 80   |
| Deployment    | subscriptions-service    | 2        | culturio-subscriptions:latest  | 80   |
| Deployment    | packages-service         | 2        | culturio-packages:latest       | 80   |
| Deployment    | apigateway               | 2        | culturio-apigateway:latest     | 80   |

#### **Services**

| Service Name              | Type        | Cluster IP    | Port(s)    |
|---------------------------|-------------|---------------|------------|
| sqlserver                 | ClusterIP   | 10.96.x.x     | 1433       |
| auth-service              | ClusterIP   | 10.96.x.x     | 80         |
| users-service             | ClusterIP   | 10.96.x.x     | 80         |
| cultureobjects-service    | ClusterIP   | 10.96.x.x     | 80         |
| subscriptions-service     | ClusterIP   | 10.96.x.x     | 80         |
| packages-service          | ClusterIP   | 10.96.x.x     | 80         |
| apigateway                | ClusterIP   | 10.96.x.x     | 80         |

#### **Ingress Configuration**

- **Host:** `culturio.local`
- **Path:** `/` → apigateway:80
- **Ingress Class:** nginx

### Running the Application with Kubernetes

#### **Prerequisites**
- Minikube installed
- kubectl installed
- Docker Desktop running
- Docker images built locally

#### **Setup Minikube cluster:**
```bash
# Start Minikube
minikube start --driver=docker --cpus=4 --memory=4096

# Or with alternative drivers:
# minikube start --driver=hyperv --cpus=4 --memory=4096
# minikube start --driver=virtualbox --cpus=4 --memory=4096

# Verify cluster is running
minikube status

# Enable ingress addon
minikube addons enable ingress

# Verify ingress is enabled
kubectl get pods -n ingress-nginx
```

#### **Build Docker images in Minikube:**
```bash
# Point Docker CLI to Minikube's Docker daemon
# PowerShell:
& minikube -p minikube docker-env --shell powershell | Invoke-Expression

# CMD:
@FOR /f "tokens=*" %i IN ('minikube -p minikube docker-env --shell cmd') DO @%i

# Verify you're connected to Minikube Docker
docker info | findstr "Name:"
# Should show: Name: minikube

# Build images
docker build -t culturio-auth:latest -f Span.Culturio.Auth\Dockerfile .
docker build -t culturio-users:latest -f Span.Culturio.Users\Dockerfile .
docker build -t culturio-cultureobjects:latest -f Span.Culturio.CultureObjects\Dockerfile .
docker build -t culturio-subscriptions:latest -f Span.Culturio.Subscriptions\Dockerfile .
docker build -t culturio-packages:latest -f Span.Culturio.Packages\Dockerfile .
docker build -t culturio-apigateway:latest -f Span.Culturio.ApiGateway\Dockerfile .

# Verify images are in Minikube
docker images | findstr culturio
```

#### **Deploy to Kubernetes:**
```bash
# 1. Create namespace
kubectl apply -f k8s\namespace.yaml

# 2. Deploy SQL Server
kubectl apply -f k8s\sqlserver\deployment.yaml
kubectl apply -f k8s\sqlserver\service.yaml

# Wait for SQL Server to be ready (60-120 seconds)
kubectl wait --for=condition=ready pod -l app=sqlserver -n culturio --timeout=120s

# 3. Create databases
kubectl exec -it -n culturio deployment/sqlserver -- /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -Q "CREATE DATABASE [Culturio.Users]; CREATE DATABASE [Culturio.CultureObjects]; CREATE DATABASE [Culturio.Subscriptions]; CREATE DATABASE [Culturio.Packages];"

# 4. Deploy microservices
kubectl apply -f k8s\auth\deployment.yaml
kubectl apply -f k8s\auth\service.yaml

kubectl apply -f k8s\users\deployment.yaml
kubectl apply -f k8s\users\service.yaml

kubectl apply -f k8s\cultureobjects\deployment.yaml
kubectl apply -f k8s\cultureobjects\service.yaml

kubectl apply -f k8s\subscriptions\deployment.yaml
kubectl apply -f k8s\subscriptions\service.yaml

kubectl apply -f k8s\packages\deployment.yaml
kubectl apply -f k8s\packages\service.yaml

kubectl apply -f k8s\apigateway\deployment.yaml
kubectl apply -f k8s\apigateway\service.yaml

# 5. Deploy ingress
kubectl apply -f k8s\ingress.yaml

# 6. Verify deployment
kubectl get all -n culturio
```

#### **Access the application:**
```bash
# Get Minikube IP address
minikube ip
# Example output: 192.168.49.2

# Add to hosts file
# Windows: C:\Windows\System32\drivers\etc\hosts (open as Administrator)
# Add line: 192.168.49.2  culturio.local

# Access the application
# Browser: http://culturio.local
# Or: http://culturio.local/swagger
```

**Alternative - Port Forwarding:**
```bash
# Forward API Gateway port to localhost
kubectl port-forward -n culturio svc/apigateway 8080:80

# Access via: http://localhost:8080
```

#### **Useful kubectl commands:**
```bash
# Get all resources in namespace
kubectl get all -n culturio

# Get services
kubectl get services -n culturio

# Get deployments  
kubectl get deployments -n culturio

# Get pods
kubectl get pods -n culturio

# Get pod details
kubectl describe pod <pod-name> -n culturio

# View logs
kubectl logs <pod-name> -n culturio

# Follow logs in real-time
kubectl logs -f <pod-name> -n culturio

# Get logs for specific service (first pod)
kubectl logs -n culturio deployment/users-service

# Execute command in pod
kubectl exec -it <pod-name> -n culturio -- /bin/bash

# Get ingress details
kubectl get ingress -n culturio
kubectl describe ingress culturio-ingress -n culturio

# Scale deployment
kubectl scale deployment auth-service --replicas=3 -n culturio

# Delete all resources in namespace
kubectl delete namespace culturio

# Restart deployment
kubectl rollout restart deployment/auth-service -n culturio
```

#### **Troubleshooting:**
```bash
# Check pod status
kubectl get pods -n culturio

# If pod is not running, check events
kubectl describe pod <pod-name> -n culturio

# Check logs for errors
kubectl logs <pod-name> -n culturio

# Check if images are available
minikube ssh
docker images | grep culturio
exit

# Restart Minikube if needed
minikube stop
minikube delete
minikube start --driver=docker --cpus=4 --memory=4096
```

### API Usage Example (Kubernetes)

**Register via Ingress:**
```bash
curl -X POST http://culturio.local/auth/register ^
  -H "Content-Type: application/json" ^
  -d "{\"firstName\":\"John\",\"lastName\":\"Doe\",\"email\":\"john@example.com\",\"username\":\"johndoe\",\"password\":\"SecurePass123!\"}"
```

**Login:**
```bash
curl -X POST http://culturio.local/auth/login ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"johndoe\",\"password\":\"SecurePass123!\"}"
```

**Get Users (with token):**
```bash
curl -X GET "http://culturio.local/users?pageSize=10&pageIndex=0" ^
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

## Technology Stack

### Backend
- **Framework:** ASP.NET Core 8.0 (Web API)
- **Authentication:** JWT Bearer Tokens
- **ORM:** Entity Framework Core 8.0
- **Database:** SQL Server (LocalDB for dev, SQL Server 2022 for containers)
- **API Gateway:** Ocelot 23.2.2
- **Password Hashing:** BCrypt.Net

### DevOps & Infrastructure
- **Containerization:** Docker & Docker Compose
- **Orchestration:** Kubernetes (Minikube)
- **Ingress Controller:** nginx-ingress
- **Version Control:** Git (Azure DevOps)
- **API Documentation:** Swagger/OpenAPI

---

## Security Notes

### Development
- JWT Secret Key stored in **User Secrets** (not committed to Git)
- Database passwords configured via environment variables
- HTTPS redirection enabled

### Docker Environment
- Secrets passed via environment variables in `docker-compose.yml`
- **⚠️ WARNING:** Current setup uses hardcoded passwords for demonstration purposes only

### Kubernetes Environment
- Secrets configured directly in deployment manifests for demonstration
- **⚠️ WARNING:** In production, use Kubernetes Secrets or external secret management

### Production Deployment Requirements
- Kubernetes Secrets for sensitive data
- Azure Key Vault or similar secret management service
- Encrypted connection strings
- Proper TLS/SSL certificate management
- Network policies and RBAC
- Resource limits and quotas

---

## Branch Structure

| Branch/Tag | Purpose                                  |
|------------|------------------------------------------|
| `main`     | Task 1 (Monolithic) & Task 2 (Microservices) |
| `DZ3`      | Task 3 (Docker & API Gateway)            |
| `DZ4`      | Task 4 (Kubernetes Deployment)           |

---

## Project Status 

### Completed
- ✅ Task 1: Monolithic REST API
- ✅ Task 2: Microservice architecture with JWT auth
- ✅ Task 3: Docker containerization and API Gateway
- ✅ Task 4: Kubernetes deployment with Ingress

---

## Contributors
- **Mia Matić** — Developer
- **Course:** ARDPSPM (Span @ FER)

---

## License
This project is part of a university course assignment and is intended for educational purposes.

---

*Last updated: February 2026*
