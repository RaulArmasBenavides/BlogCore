# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**BlogCore** is an ASP.NET Core 7 MVC web application for managing blog content with articles, categories, sliders, and user management. It uses a layered architecture with Entity Framework Core for data persistence and SQL Server as the database.

Tech stack:
- **Framework:** ASP.NET Core 7
- **Database:** SQL Server (via Entity Framework Core)
- **Authentication:** ASP.NET Identity
- **Architecture:** Layered (MVC with Area-based organization)
- **Containerization:** Docker + Docker Compose

## Build & Development Commands

### Prerequisites
- .NET 7 SDK
- SQL Server (local or via Docker)
- Docker & Docker Compose (for containerized development)

### Local Development (without Docker)

**Build the solution:**
```powershell
cd BlogCore
dotnet build BlogCore.sln
```

**Run the web application:**
```powershell
cd BlogCore
dotnet run --project BlogCore/BlogCore.csproj
```

The application will be available at `https://localhost:7000` (or the port specified in launchSettings.json).

**Run Entity Framework migrations:**
```powershell
cd BlogCore
dotnet ef database update --project BlogCore.AccesoDatos
```

**Create a new migration:**
```powershell
cd BlogCore
dotnet ef migrations add MigrationName --project BlogCore.AccesoDatos --startup-project BlogCore
```

### Docker Development

**Build and run with Docker Compose:**
```powershell
cd BlogCore
docker compose build
docker compose up -d
```

The application will be available at `http://localhost:8000`.

**Common Docker Compose commands:**
```powershell
# Rebuild and restart the web service only
docker compose up -d --no-deps --build blogcore

# Rebuild the service
docker compose build blogcore

# Recreate if image changed
docker compose up -d blogcore

# Restart without rebuilding
docker compose restart blogcore

# Stop all services
docker compose stop

# Stop and remove all containers and volumes (including database)
docker compose down -v
```

**Database:** SQL Server runs in a container with data volume `sql_data`. Default credentials:
- User: `sa`
- Password: `P@ssw0rd?`
- Port: `11433` (mapped from 1433)

### Demo Credentials

```
Email: raul.armas@intelica.com
Password: marceloPERU!3
```

## Project Structure

### Solution Organization

```
BlogCore/
├── BlogCore/                    # Main ASP.NET Core web project
├── BlogCore.AccesoDatos/        # Data access layer (EF Core + repositories)
├── BlogCore.Models/             # Domain models and entities
├── BlogCore.Utilidades/         # Utility classes and helpers
├── scripts/                     # Database initialization scripts
├── docker-compose.yml           # Multi-container orchestration
├── BlogCore.sln                 # Solution file
└── Dockerfile                   # Container image for the web app
```

### Main Project (BlogCore) Structure

```
BlogCore/
├── Areas/                       # Area-based feature organization
│   ├── Admin/                   # Admin panel features
│   ├── Cliente/                 # Client-facing features
│   └── Identity/                # Identity/authentication pages
├── Controllers/                 # MVC controllers
├── Views/                       # Razor views (MVC templates)
├── wwwroot/                     # Static files (CSS, JS, images)
│   └── imagenes/
│       ├── articulos/           # Article images
│       └── sliders/             # Slider images
├── Migrations/                  # EF Core migration files
├── Extensions/                  # Dependency injection and extension methods
├── Properties/                  # Project properties and settings
├── Program.cs                   # Application entry point and configuration
└── appsettings.json             # Configuration settings
```

### Key Layer Responsibilities

**BlogCore.AccesoDatos (Data Access)**
- Entity Framework Core DbContext setup
- Repository pattern implementation
- Database migrations
- Connection string configuration
- Identity user store configuration

**BlogCore.Models (Domain Models)**
- Entity/model classes (Articles, Categories, Users, etc.)
- ApplicationUser (extends IdentityUser)
- Data validation attributes

**BlogCore.Utilidades (Utilities)**
- Helper classes and extension methods
- Shared logic across layers

**BlogCore (Presentation)**
- MVC Controllers
- Razor Views
- Static assets
- Program.cs: dependency injection and middleware configuration

## Key Patterns & Configuration

### Dependency Injection (Program.cs)
The application configures services in Program.cs using extension methods located in `Extensions/`:
- `AddPersistence()` - EF Core and database configuration
- `AddCustomResponseCompression()` - Response compression
- `AddCustomHealthChecks()` - Health check endpoints
- `AddControllersWithViews()` - MVC support
- `ConfigureCors()` - CORS configuration
- `AddHttpContextAccessor()` - HTTP context access

### Areas
The application uses ASP.NET Areas for feature organization:
- **Admin** - Administrative features (likely for content management)
- **Cliente** - Client/public-facing features (default routing area)
- **Identity** - Authentication and account management pages

Default routing pattern: `{area=Cliente}/{controller=Home}/{action=Index}/{id?}`

### Authentication
Uses ASP.NET Identity with Entity Framework Core stores:
- User sign-in does **not** require email confirmation (`RequireConfirmedAccount = false`)
- Supports standard Identity pages (login, register, password reset, etc.)

### Environment-Specific Behavior
- **Development:** Database migration endpoint enabled for EF Core diagnostics
- **Production:** Exception handling via `/Home/Error` endpoint

### Database Configuration
- Connection string: Configured in appsettings.json and injected from environment variables in Docker
- SQL Server via EF Core
- Identity tables and migrations managed by EF Core

### Static Files & Images
Static files are served from `wwwroot/`:
- Article images stored in `wwwroot/imagenes/articulos/`
- Slider images stored in `wwwroot/imagenes/sliders/`

## Important Development Notes

- **Null safety:** Main project uses `#nullable enable`, AccesoDatos uses `#nullable enable`, Models use `#nullable disable`
- **Implicit usings:** All projects use implicit global usings
- **Docker:** The Dockerfile is located inside the `BlogCore/` folder (not at repo root), and the docker-compose.yml is in the `BlogCore/` solution folder
- **Health checks:** Application exposes a `/health` endpoint for monitoring
- **CORS:** Custom CORS configuration is applied to the pipeline

## Using the /net-expert Skill

For .NET-specific tasks (Entity Framework Core, database migrations, architectural changes, dependency injection, ASP.NET Identity integration, validation, refactoring), consider using the `/net-expert` skill to leverage specialized expertise.
