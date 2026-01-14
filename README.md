# Span.Culturio

Culturio platform for managing cultural institutions, users, subscriptions, and visits.

## Overview

This repository contains the implementations of **Task 1** and **Task 2** for the course  
**Agilni razvoj digitalnih platformi s pomoću mikroservisa (ARDPSPM)**  
(Span @ FER).

The project is developed incrementally through course assignments:

- **Task 1** focuses on building a **monolithic .NET REST API**
- **Task 2** refactors the monolith into a **microservice-based architecture**

Both implementations are intentionally kept in this repository for reference, comparison, and further development.

## Repository Structure
culturio-project/

├── Span.Culturio -> Task 1 – Monolithic application

└── Span.Culturio.Microservices -> Task 2 – Microservice architecture


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


## Task 2 – Microservice Architecture

### Assignment Description

The goal of Task 2 is to refactor the monolithic application from Task 1 into a **microservice-based system**.

Each microservice represents a separate domain, includes cross-cutting concerns, and is connected to its own database.

### Implemented Requirements

- Created a new solution: **Span.Culturio.Microservices**
- Split the monolithic application into independent services
- Implemented:
  - authentication and authorization,
  - user role validation,
  - input validation,
  - logging
- Created separate databases and connected them to the appropriate services
- Reused and reorganized the code from Task 1 into multiple projects

### Implemented Services

- **Span.Culturio.Auth**
- **Span.Culturio.Users**
- **Span.Culturio.CultureObjects**
- **Span.Culturio.Subscriptions**
- **Span.Culturio.Packages**

### Databases

| Service            | Database                |
|--------------------|-------------------------|
| Auth, Users        | Culturio.Users          |
| CultureObjects     | Culturio.CultureObjects |
| Subscriptions      | Culturio.Subscriptions  |
| Packages           | Culturio.Packages       |

### API Summary (Task 2)

The API remains aligned with the OpenAPI specification from Task 1 but is distributed across multiple services.

**Auth Service**
- `POST /auth/register`
- `POST /auth/login`

**Users Service**
- `GET /users`
- `GET /users/{id}`

**Culture Objects Service**
- `POST /culture-objects`
- `GET /culture-objects`
- `GET /culture-objects/{id}`

**Subscriptions Service**
- `POST /subscriptions`
- `GET /subscriptions`
- `POST /subscriptions/activate`
- `POST /subscriptions/track-visit`

**Packages Service**
- `GET /packages`


## Project Status
Future assignments will further extend the platform, and this README will be updated accordingly.
