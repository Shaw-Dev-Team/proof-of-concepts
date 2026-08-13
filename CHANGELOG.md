# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.2.0] - 2026-08-13

### Added

#### F-002 — Workflow Definition CRUD + Versioning API/Service
- Implemented full REST API endpoints for workflow definition lifecycle: Create, Read, Update, Delete operations
- Built `WorkflowDefinitionService` with versioning logic and immutable snapshot handling
- Created request/response contracts for API consumers (`CreateWorkflowDefinitionRequest`, etc.)
- Integrated service layer into minimal API routing (`WorkflowDefinitionEndpoints.cs`)
- All CRUD operations respect definition versioning: new versions are created atomically, prior versions remain immutable

**Modules/Files Affected:**
- `src/backend/WorkflowPlatform.Api/Services/` — new service layer:
  - `WorkflowDefinitionService.cs` — CRUD and versioning business logic
- `src/backend/WorkflowPlatform.Api/Contracts/` — new request/response types:
  - `CreateWorkflowDefinitionRequest.cs` — and related contract classes
- `src/backend/WorkflowPlatform.Api/Endpoints/WorkflowDefinitionEndpoints.cs` — route handlers
- `src/backend/WorkflowPlatform.Api/Program.cs` — service registration and endpoint mapping
- `src/backend/WorkflowPlatform.Api.Tests/WorkflowDefinitionServiceTests.cs` — integration tests for CRUD and versioning
- Sample `WeatherForecast` endpoint removed (scaffold-only, never part of the API contract)

### Changed

#### F-001 Amendment — Composite Node Key + Cascade Schema Change
- **Breaking Schema Change**: `WorkflowNode` primary key changed from `NodeId` alone to composite `(NodeId, WorkflowDefinitionId)` to enforce per-definition node uniqueness and enable safe node reuse across definition versions
- `NodeExecution` entity gained `WorkflowDefinitionId` column to maintain referential integrity and support tracking execution across versioned definitions
- `TaskHandlerReference` entity gained `WorkflowDefinitionId` column for the same reason
- Generated new migration: `CompositeNodeKey.cs` reflecting the schema change
- All entity relationships updated to support the composite key structure

**Modules/Files Affected:**
- `src/backend/WorkflowPlatform.Api/Domain/WorkflowNode.cs` — primary key configuration
- `src/backend/WorkflowPlatform.Api/Domain/NodeExecution.cs` — added `WorkflowDefinitionId` column
- `src/backend/WorkflowPlatform.Api/Domain/TaskHandlerReference.cs` — added `WorkflowDefinitionId` column
- `src/backend/WorkflowPlatform.Api/Data/WorkflowPlatformDbContext.cs` — entity configuration updates
- `src/backend/WorkflowPlatform.Api/Migrations/` — new `CompositeNodeKey.cs` migration

### Testing & Verification

**Happy Path:**
- Create a workflow definition with a Start → Task → End node graph via POST endpoint
- Retrieve the definition by ID via GET endpoint and confirm all nodes and connections are intact

**Edge Case:**
- Create a new version of an existing definition, reusing the same node IDs from the prior version
- Confirm both versions coexist independently in the database
- Confirm the prior version's row is byte-for-byte unchanged after the new version is created

**Regression-Sensitive:**
- Query all versions of a workflow definition by name via GET endpoint
- Confirm versions are returned in correct ordering (oldest to newest or newest to oldest as designed)
- Confirm no data loss or mutation in any prior version's rows

### Breaking Changes
- **F-001 Amendment**: `WorkflowNode` primary key changed to composite `(NodeId, WorkflowDefinitionId)`. Any existing database with F-001's baseline schema must run the `CompositeNodeKey` migration. Code that assumes `NodeId` is globally unique per table will fail and must be updated to account for per-definition scoping.
- `NodeExecution` and `TaskHandlerReference` schema changed (new column). Migration is required.
- Removal of sample `WeatherForecast` endpoint — it was never a real API contract, only scaffolding.

---

## [1.1.0] - 2026-08-13

### Added

#### F-001 — Domain/Data Schema + EF Core Migrations
- Defined core domain entities: `WorkflowDefinition`, `WorkflowNode`, `NodeConnection`, `WorkflowInstance`, `NodeExecution`
- Created enums: `WorkflowNodeType`, `NodeExecutionStatus`, `WorkflowInstanceStatus`
- Implemented versioned workflow definitions with immutable snapshots on instance creation
- Generated `InitialSchema` migration replacing the placeholder `InitialEmpty` migration
- Added comprehensive data constraints: uniqueness on `(Name, Version)` pairs, foreign key integrity, cascade delete rules
- Created unit test project `WorkflowPlatform.Api.Tests` with 13 passing tests

**Modules/Files Affected:**
- `src/backend/WorkflowPlatform.Api/Domain/` — 6 new entity classes:
  - `WorkflowDefinition.cs` — workflow template with versioning
  - `WorkflowNode.cs` — individual node with metadata
  - `NodeConnection.cs` — explicit edges between nodes
  - `WorkflowInstance.cs` — runtime instance bound to a specific definition version
  - `NodeExecution.cs` — per-node execution tracking
  - Plus 3 enum files: `WorkflowNodeType.cs`, `NodeExecutionStatus.cs`, `WorkflowInstanceStatus.cs`
- `src/backend/WorkflowPlatform.Api/Data/WorkflowPlatformDbContext.cs` — modified to register entities and configure relationships
- `src/backend/WorkflowPlatform.Api/Migrations/` — new `InitialSchema.cs` migration with complete schema
- `src/backend/WorkflowPlatform.slnx` — added test project reference
- `src/backend/WorkflowPlatform.Api.Tests/` — new test project with 13 tests covering entity creation, relationships, and constraints

### Testing & Verification

**Happy Path:**
- `dotnet build` — solution builds without errors
- `dotnet test` — all 13 tests pass

**Edge Case:**
- A `WorkflowDefinition` with zero nodes (no `WorkflowNode` children) persists correctly to the database

**Regression-Sensitive:**
- Two `WorkflowDefinition` rows with identical `Name` but different `Version` values coexist independently without constraint violation
- A `WorkflowInstance`'s `DefinitionVersion` snapshot remains unchanged when a new `WorkflowDefinition` version is subsequently created

### Breaking Changes
None — initial domain schema implementation with no prior consumers.

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
