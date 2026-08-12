# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.0.0] - 2026-08-13

### Added

#### F-013 — Angular Workspace Scaffolding
- Initialized Angular 17+ standalone component workspace with TypeScript strict mode
- Configured Angular Material design system integration
- Added ESLint configuration (`eslint.config.js`)
- Created application structure: `src/app/` with root component, routing, and styling
- Includes TypeScript and SCSS configuration files

**Modules/Files Affected:**
- `src/frontend/` — new Angular workspace with:
  - `package.json` — project dependencies
  - `angular.json` — workspace configuration
  - `tsconfig.json`, `tsconfig.app.json`, `tsconfig.spec.json` — TypeScript configuration
  - `src/app/` — application entry point with routes, config, and styles
  - `eslint.config.js` — code quality standards

#### F-014 — ASP.NET Core Web API Scaffolding
- Created .NET 10 LTS solution (`WorkflowPlatform.slnx`)
- Scaffolded ASP.NET Core Web API project (`WorkflowPlatform.Api`)
- Configured minimal APIs with proper routing
- Added application settings for Development and Production environments
- Created HTTP request testing file (`WorkflowPlatform.Api.http`)

**Modules/Files Affected:**
- `src/backend/` — new .NET 10 solution with:
  - `WorkflowPlatform.slnx` — solution file
  - `WorkflowPlatform.Api/` — Web API project with:
    - `Program.cs` — application entry point and middleware configuration
    - `appsettings.json`, `appsettings.Development.json` — environment configuration
    - `WorkflowPlatform.Api.http` — HTTP request definitions
    - `WorkflowPlatform.Api.csproj` — project manifest

#### F-015 — SQL Server 2022 / EF Core Connection Setup
- Integrated Entity Framework Core with SQL Server database provider
- Created `DbContext` configuration (`WorkflowPlatformDbContext.cs`)
- Generated initial (empty) migration (`20260812223313_InitialEmpty.cs`)
- Configured dependency injection for database context
- Set up migrations infrastructure

**Modules/Files Affected:**
- `src/backend/WorkflowPlatform.Api/Data/` — database abstraction layer:
  - `WorkflowPlatformDbContext.cs` — Entity Framework Core context
- `src/backend/WorkflowPlatform.Api/Migrations/` — migration tracking:
  - `20260812223313_InitialEmpty.cs` — initial schema migration
  - `20260812223313_InitialEmpty.Designer.cs` — migration metadata
  - `WorkflowPlatformDbContextModelSnapshot.cs` — current model snapshot

### Testing & Verification

**Happy Path (both succeed):**
- `ng build` — Angular production build completes without errors
- `ng serve` — Angular development server starts and hot-reload functions correctly
- `dotnet build` — .NET solution builds without errors
- `dotnet run` — ASP.NET Core Web API starts and listens on configured ports

**Edge Case:**
- `dotnet ef migrations add <MigrationName>` — Entity Framework Core migration tooling works and generates migration files correctly
- *Note:* `dotnet ef database update` currently fails due to documented SQL Server environment gap (see PM-001 in project management) — expected and tracked

**Regression-Sensitive:**
- None — this is the initial scaffolding commit; no prior behavior exists to regress against

### Breaking Changes
None — initial scaffolding, no backward compatibility concerns.

---

## Unreleased

(Upcoming features and fixes will be documented here)
