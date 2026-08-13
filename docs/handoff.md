# Handoff: Workflow Visualization and Execution Platform POC

> Tranche: v1
> Last updated: 2026-08-13
> Project Management: docs/project-management.md
> Architecture: docs/architecture.md

## What Was Done

Phase 0 (project scaffolding) is complete, and Phase 1 (domain & data layer) is now also complete:

- **F-013 — Angular workspace scaffolding**: new Angular 22 + Angular Material 22 workspace at `src/frontend/`, standalone components, ESLint + Prettier configured. Base Material toolbar shell only — no feature UI yet.
- **F-014 — ASP.NET Core Web API scaffolding**: new .NET 10 LTS solution `src/backend/WorkflowPlatform.slnx` with Web API project `WorkflowPlatform.Api`, default minimal-API template (`/weatherforecast` endpoint), EF Core + SQL Server provider package references added.
- **F-015 — SQL Server 2022 / EF Core connection setup**: `WorkflowPlatformDbContext` registered via `AddDbContext`, connection string now targets `(localdb)\MSSQLLocalDB` (see PM-001 resolution below), automatic migration-on-startup via `Database.Migrate()` in `Program.cs`.
- **F-001 — Domain/data schema + EF Core migrations against SQL Server 2022**: 6 EF Core entities (`WorkflowDefinition`, `Node`, `Connection`, `WorkflowInstance`, `NodeExecution`, `TaskHandlerReference`) + 3 enums (`NodeType`, `NodeExecutionState`, `WorkflowInstanceStatus`) added under `src/backend/WorkflowPlatform.Api/Domain/`. `WorkflowPlatformDbContext` exposes `DbSet<T>` for all six entities with full `OnModelCreating` fluent configuration (cascade/restrict delete rules split to avoid SQL Server's multiple-cascade-paths error). Node graph relationships use normalized adjacency-style navigation collections (`IncomingConnections`/`OutgoingConnections` via `Connection`, `CurrentNodes` via an implicit many-to-many join table) per ADR-003 — not raw ID arrays or JSON blobs. New migration `20260812234443_InitialSchema` replaces the old empty `InitialEmpty` migration; verified for real by applying to a freshly dropped `(localdb)\MSSQLLocalDB` database and confirming all 6 tables + the `WorkflowInstanceCurrentNodes` join table exist via sqlcmd. New xUnit test project `src/backend/WorkflowPlatform.Api.Tests/` added to `WorkflowPlatform.slnx` — 13 tests covering entity CRUD, nullability/required-field enforcement, relationship loading, and enum completeness; all 13 pass (verified by Integration Agent). Documentation-only fix: architecture.md §4 Entity Schema corrected to describe the actual navigation-collection shape instead of raw ID arrays.

## Current State

- Frontend (`src/frontend/`) and backend (`src/backend/`) both build, lint, format, and test clean.
- Full domain/data layer exists and is EF Core-mapped: 6 entities + 3 enums, with the `InitialSchema` migration verified for real against `(localdb)\MSSQLLocalDB` (see PM-001 resolution — a full SQL Server 2022 Database Engine is still not installed on this machine, but LocalDB is confirmed sufficient for local dev). 13 backend unit tests pass.
- No feature APIs or UI screens (Workflow Definition CRUD, Designer graph editor, etc.) have been written yet — Phase 1 was schema/migrations only.
- Frontend unchanged this round; still builds/lints/tests clean per Integration validation.

## What's Next

- **Phase 2 — Workflow Definition Service & API** (F-002): CRUD + versioning API/service for workflow definitions, independent of any running instance. Depends on F-001 (now satisfied) and F-014 (already Done) — both dependencies are now met, so F-002 is unblocked.

## Important Context

- **Folder layout**: two project folders under `src/` — `src/frontend/` (Angular workspace) and `src/backend/` (ASP.NET Core solution, `WorkflowPlatform.slnx`).
- **SQL Server environment (PM-001 — Resolved)**: no full SQL Server 2022 Database Engine is installed on this machine, only `Microsoft SQL Server 2025 LocalDB` and client tooling. `appsettings.Development.json` now targets `(localdb)\MSSQLLocalDB` instead of `localhost`; `dotnet ef database update` is verified working for real against LocalDB, including the `InitialSchema` migration used for F-001. LocalDB is confirmed sufficient for local development going forward — revisit only if a non-dev environment needs a real SQL Server 2022 instance.
- **Domain modeling assumptions carried into F-002+**: `WorkflowDefinition.Status` is kept as a free-form nullable string (no enum/lifecycle defined for it in Architecture §4); `ManualApproval` is deliberately excluded from the `NodeType` enum (architecture explicitly models it as a Task node, not a distinct primitive); several free-form fields (`Configuration`, `Metadata`, `Result`, `EvaluationOutcome`, `ExternalMetadata`) are stored as nullable strings pending a defined sub-schema.
