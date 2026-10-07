# Inventory API

A Restful Web API built with ASP.NET Core (.NET 10) following Clean Architecture principles.

## Tech Stack

- .NET 10
- ASP.NET Core Web API (Controllers)
- Entity Framework Core
- PostgreSQL 17
- Docker & Docker Compose
- VS Code

## Architecture

Clean Architecture with 4 layers:

- **Inventory.Api** - HTTP layer (Controllers, Middleware, DI setup)
- **Inventory.Application** - Business logic (Services, DTOs, Interfaces)
- **Inventory.Domain** - Core entities and business rules
- **Inventory.Infrastructure** - Data access (EF Core, Repositories)

Dependency direction: `Api → Application → Domain`, `Infrastructure → Application + Domain`.

## Getting Started

### Prerequisites

- .NET 10 SDK
- Docker Desktop
- VS Code with C# Dev Kit

### Run
```bash
docker compose up -d
dotnet run --project src/Inventory.Api
```

## Project Status

### Phase 1 — Foundation (Done)

* Clean Architecture skeleton (4 layers)
* Product entity with encapsulated state
* EF Core + PostgreSQL + Docker
* `GET /api/products/{id}` endpoint
* EF Core migration pipeline

### Phase 2 — Full CRUD + Quality (In Progress)

* Application DI extension
* Create / Read / Update / Delete endpoints
* Pagination
* FluentValidation
* Global exception handling (ProblemDetails)
* Structured logging (Serilog + Seq)

### Roadmap

* Phase 3 — Advanced REST (PATCH, content negotiation, HATEOAS, versioning, caching, rate limiting)
* Phase 4 — Security (JWT, authorization policies)
* Phase 5 — Testing (xUnit, integration tests, Testcontainers)
* Phase 6 — CI/CD (GitHub Actions, Docker build, deployment)
