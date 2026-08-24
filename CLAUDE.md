# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**BlogCore** is an ASP.NET Core 10 MVC web application for managing blog content with articles, categories, sliders, and user management. It uses a layered architecture with Entity Framework Core for data persistence and SQL Server as the database, implementing the Unit of Work and Repository patterns for data access.

Tech stack:
- **Framework:** ASP.NET Core 10 (.NET 10)
- **Database:** SQL Server (via Entity Framework Core)
- **Authentication:** ASP.NET Identity
- **Architecture:** Layered with Unit of Work + Repository pattern
- **Containerization:** Docker + Docker Compose

## Build & Development Commands

### Prerequisites
- .NET 10 SDK
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
- Database name: `CONSULTORIO2`

### Demo Credentials

```
Email: raul.armas@intelica.com
Password: marceloPERU!3
```

## Project Structure

### Solution Organization

```
BlogCore/
├── BlogCore/                    # Main ASP.NET Core web project (MVC + Views)
├── BlogCore.AccesoDatos/        # Data access layer (EF Core, Unit of Work, Repository pattern)
├── BlogCore.Models/             # Domain models and entities
├── BlogCore.Utilidades/         # Utility classes and helpers
├── scripts/                     # Database initialization scripts (SQL Server Dockerfile)
├── docker-compose.yml           # Multi-container orchestration
├── BlogCore.sln                 # Solution file
└── BlogCore/Dockerfile          # Container image for the web app (multi-stage build)
```

### Main Project (BlogCore) Structure

```
BlogCore/
├── Areas/                       # Area-based feature organization
│   ├── Admin/                   # Admin panel features (ArticulosController, CategoriasController, SlidersController, UsuariosController)
│   ├── Cliente/                 # Client-facing features (HomeController)
│   └── Identity/                # Identity/authentication pages (scaffolded Razor Pages)
├── Views/                       # Razor views (MVC templates)
├── wwwroot/                     # Static files (CSS, JS, images)
│   └── imagenes/
│       ├── articulos/           # Article images uploaded by admin
│       └── sliders/             # Slider images uploaded by admin
├── Extensions/                  # Dependency injection and extension methods
│   ├── DependencyInjection.cs   # AddPersistence() - DbContext & repository setup
│   ├── ApplicationServicesExtensions.cs
│   ├── CorsConfigurationExtension.cs
│   └── HealthCheckExtensions.cs
├── Migrations/                  # EF Core migration files
├── Properties/                  # Project properties and launch settings
├── Program.cs                   # Application entry point and service configuration
└── appsettings.json             # Configuration settings and connection strings
```

### Data Access Layer (BlogCore.AccesoDatos)

```
BlogCore.AccesoDatos/
├── Data/
│   ├── ApplicationDbContext.cs  # EF Core DbContext (inherits from IdentityDbContext)
│   └── Repository/
│       ├── ContenedorTrabajo.cs # Unit of Work implementation (IContenedorTrabajo)
│       ├── Repository<T>.cs     # Generic repository base class
│       ├── [Entity]Repository.cs # Specific repositories (ArticuloRepository, CategoriaRepository, etc.)
│       └── IRepository/          # Repository interfaces
```

The **Unit of Work pattern** (`ContenedorTrabajo`) aggregates repositories and manages DbContext lifecycle. Access data via `IContenedorTrabajo` injected into controllers:
```csharp
public IContenedorTrabajo _contenedor { get; private set; }
// Use: _contenedor.Articulo, _contenedor.Categoria, _contenedor.Usuario, _contenedor.Slider
// Always call _contenedor.Save() after modifications
```

### Domain Models

Main entities in `BlogCore.Models`:
- **Articulo** - Blog articles with name, description, creation date, image URL, and category foreign key
- **Categoria** - Article categories
- **Slider** - Homepage slider images
- **ApplicationUser** - Extends `IdentityUser` for custom user properties

## Key Patterns & Architecture

### Dependency Injection (Program.cs)
Services configured via extension methods in `Extensions/`:
- `AddPersistence()` - DbContext setup, repository registration, Unit of Work registration
- `AddCustomResponseCompression()` - Response compression middleware
- `AddCustomHealthChecks()` - Health check endpoints (exposed at `/health`)
- `AddControllersWithViews()` - MVC support
- `ConfigureCors()` - CORS configuration
- `AddHttpContextAccessor()` - HTTP context access for getting current user

### Repository & Unit of Work Pattern
- Generic `Repository<T>` provides base CRUD operations
- Entity-specific repositories (ArticuloRepository, etc.) can override or extend base behavior
- **ContenedorTrabajo** (Unit of Work) aggregates all repositories and manages `SaveChanges()`
- Repositories are registered in DI as both generic `IRepository<T>` and the specific interfaces

### Areas & Routing
- **Admin** area (`/admin`) - Content management controllers
- **Cliente** area (`/cliente`) - Public-facing pages (default area)
- **Identity** area - Scaffolded Razor Pages for authentication
- Default route: `{area=Cliente}/{controller=Home}/{action=Index}/{id?}`

### Authentication & Authorization
- ASP.NET Identity with Entity Framework Core stores
- Email confirmation **not required** (`RequireConfirmedAccount = false`)
- User sign-in via standard Identity pages

### Database & Migrations
- SQL Server via EF Core ORM
- Connection string: `ConexionSQL` in `appsettings.json` (overridable via Docker environment variables)
- All entity mappings and Identity tables managed by EF Core migrations
- To run migrations: `dotnet ef database update --project BlogCore.AccesoDatos`

### Environment-Specific Behavior
- **Development:** Database migration diagnostics endpoint enabled, developer exception page
- **Production:** Centralized error handling via `/Home/Error` endpoint

## Important Development Notes

- **Null safety:** BlogCore uses `#nullable enable`, AccesoDatos uses `#nullable enable`, Models use `#nullable disable`
- **Implicit usings:** All projects enable implicit global usings
- **Docker:** Dockerfile is in `BlogCore/BlogCore/` (multi-stage build), docker-compose.yml in `BlogCore/`
- **Static files:** Served from `wwwroot/`; images are in `wwwroot/imagenes/{articulos,sliders}/`
- **Health endpoint:** `/health` available for monitoring
- **CORS:** Configured via `CorsConfigurationExtension` and applied in the pipeline

## Known Gaps & Technical Debt

- **No unit/integration tests** - Consider adding an xUnit or MSTest project
- **No async repository methods** - Current repository implementation uses synchronous EF Core calls
- **Credentials in config files** - Connection string passwords are visible; use User Secrets in development and environment variables in production
- **No input validation middleware** - Consider adding Fluent Validation or data annotations validation pipeline
- **No API documentation** - Consider adding Swagger/OpenAPI for REST endpoints if exposing APIs
- **Models use nullable disable** - BlogCore.Models has `#nullable disable`; consider enabling for type safety

## Async/Await Implementation (SEMANA 1 - COMPLETADO)

**All repository methods are now async:**
- `GetAsync(id)`, `GetAllAsync()`, `GetFirstOrDefaultAsync()` 
- `AddAsync()`, `RemoveAsync()`, `UpdateAsync()`
- `SaveAsync()` added to Unit of Work
- All controllers updated to use `await` and `async Task<IActionResult>`

**Benefits:**
- No thread pool starvation under high load
- Better scalability and responsiveness
- Proper async database operations with EF Core

## Securing Credentials (SEMANA 1 - COMPLETADO)

### Local Development (User Secrets)
Store sensitive data locally without committing to git:

```powershell
cd BlogCore
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:ConexionSQL" "server=localhost;database=CONSULTORIO2;User Id=sa;Password=P@ssw0rd?;TrustServerCertificate=True"
```

**Note:** `appsettings.Development.json` contains local credentials and is NOT in .gitignore (for convenience), but in production, this file should be removed and credentials passed via environment variables.

### Production Deployment (Environment Variables)
Set environment variable before running:

```bash
export ConnectionStrings__ConexionSQL="Server=db,1433;Database=CONSULTORIO2;User Id=sa;Password=P@ssw0rd?;Encrypt=False"
dotnet run
```

### Docker Deployment
Set via `.env` file or docker-compose override:

```bash
cp .env.example .env
# Edit .env with production credentials
docker compose --env-file .env up -d
```

### Configuration Hierarchy (ASP.NET Core)
1. User Secrets (development only)
2. Environment variables
3. `appsettings.{Environment}.json` 
4. `appsettings.json` (fallback, empty connection string)

**Security Best Practice:** Never commit `.env`, `secrets.json`, or `appsettings.Production.json` to version control.

## Testing (SEMANA 2 - COMPLETADO)

**Unit test project created:** `BlogCore.Tests` (xUnit + Moq + FluentValidation)

### Running Tests
```powershell
cd BlogCore
dotnet test BlogCore.Tests
```

### Test Coverage
- `RepositoryTests.cs` - In-memory database tests for CRUD operations
- `ValidatorTests.cs` - FluentValidation rule tests

### Project Structure
```
BlogCore.Tests/
├── RepositoryTests.cs    # Tests for async repository methods
├── ValidatorTests.cs     # Tests for model validators
└── BlogCore.Tests.csproj
```

## Input Validation with FluentValidation (SEMANA 2 - COMPLETADO)

**Validators implemented** for all main entities:

### Available Validators
- **ArticuloValidator** - Validates name, description, category
- **CategoriaValidator** - Validates category name
- **SliderValidator** - Validates slider name and image URL

### How to Use in Controllers
```csharp
public async Task<IActionResult> Create(Articulo articulo)
{
    var validator = new ArticuloValidator();
    var result = await validator.ValidateAsync(articulo);
    
    if (!result.IsValid)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
        return View(articulo);
    }
    // Process valid data
}
```

### Validators Auto-Registered
FluentValidation validators are automatically registered in `Program.cs`:
```csharp
builder.Services.AddValidatorsFromAssemblyContaining<ArticuloValidator>();
```

### Custom Validation Rules
All validators enforce:
- **Required fields:** Name, description, category
- **Length constraints:** Min/max character limits
- **Value constraints:** Category ID > 0

## Using the /net-expert Skill

For .NET-specific tasks (Entity Framework Core, database migrations, repository pattern enhancements, dependency injection, ASP.NET Identity integration, validation, async/await patterns, or architectural changes), use the `/net-expert` skill.
