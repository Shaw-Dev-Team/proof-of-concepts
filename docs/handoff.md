# Handoff: Workflow Visualization and Execution Platform POC

> Tranche: v1
> Last updated: 2026-08-13
> Project Management: docs/project-management.md
> Architecture: docs/architecture.md

## What Was Done

Phase 0 (project scaffolding) is complete:

- **F-013 — Angular workspace scaffolding**: new Angular 22 + Angular Material 22 workspace at `src/frontend/`, standalone components, ESLint + Prettier configured. Base Material toolbar shell only — no feature UI yet.
- **F-014 — ASP.NET Core Web API scaffolding**: new .NET 10 LTS solution `src/backend/WorkflowPlatform.slnx` with Web API project `WorkflowPlatform.Api`, default minimal-API template (`/weatherforecast` endpoint), EF Core + SQL Server provider package references added.
- **F-015 — SQL Server 2022 / EF Core connection setup**: empty `WorkflowPlatformDbContext` registered via `AddDbContext`, connection string in appsettings targeting `localhost` with integrated auth, initial empty EF Core migration generated (`20260812223313_InitialEmpty`).

## Current State

- Frontend (`src/frontend/`) and backend (`src/backend/`) scaffolds both exist, build, lint, and format clean.
- No feature code (domain entities, APIs, UI screens) has been written yet — this is scaffolding only.
- EF Core migration tooling (`dotnet ef migrations add`) works and confirms the project builds and connects correctly, but `dotnet ef database update` against a real SQL Server 2022 instance has **not** been verified — see Important Context below.

## What's Next

- **Phase 1 — Domain & data layer** (F-001): entity schema (WorkflowDefinition, Node, Connection, WorkflowInstance, NodeExecution, TaskHandlerReference) as EF Core-mapped tables, with migrations applied cleanly to a real SQL Server 2022 instance.
- **Phase 1 is blocked on PM-001 being resolved first** — real migration verification (`dotnet ef database update`) cannot happen until a real SQL Server 2022 Database Engine is installed/provisioned, since the only local option today is SQL Server 2025 LocalDB.

## Important Context

- **Folder layout**: two project folders under `src/` — `src/frontend/` (Angular workspace) and `src/backend/` (ASP.NET Core solution, `WorkflowPlatform.slnx`).
- **SQL Server environment gap (PM-001)**: no SQL Server 2022 Database Engine is installed on this machine, only `Microsoft SQL Server 2025 LocalDB` and client tooling. `dotnet ef database update` genuinely failed against `localhost` during F-015. This must be resolved (install/point the connection string at a real SQL Server 2022 instance) before Phase 1 migrations can be verified for real.
