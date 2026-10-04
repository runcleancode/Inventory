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
