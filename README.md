# Inventory API

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-4169E1?logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)
![Serilog](https://img.shields.io/badge/Serilog-2.12-1E90FF?logo=nuget&logoColor=white)

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

### Phase 2 — Full CRUD + Quality (Done)

* Full CRUD endpoints (Get by id, Get paged, POST, PUT, DELETE)
* Off-set based pagination with 'PagedResult<T>'
* FluentValidation with global ValidationFilter
* Global exception handling with ProblemDetails (RFC 7807)
* Structured logging with Serilog + Seq
* Application and Infrastructure DI extensions
* Manual mapping via extension methods (no Automapper)
* Conventional Commits with scoped messages

### Roadmap

* Phase 3 — Advanced REST (PATCH, content negotiation, HATEOAS, versioning, caching, rate limiting)
* Phase 4 — Security (JWT, authorization policies, refresh tokens)
* Phase 5 — Testing (xUnit, integration tests, Testcontainers)
* Phase 6 — CI/CD (GitHub Actions, Docker build, deployment)
