# ADR-002: Backend Framework — ASP.NET Core (.NET 10 LTS) Web API

> Status: ACCEPTED
> Date: 2026-08-12
> Architecture: docs/architecture.md (v3)

## Context
The user specified SQL Server 2022 as the database and asked for a simple frontend + backend split, and confirmed .NET Core 10 (the current LTS release) as the backend runtime. This repository's existing Copilot instructions and installed skills are strongly oriented toward .NET (ASP.NET REST API guidelines, .NET upgrade tooling, C#/.NET-specific skills), and ASP.NET Core pairs natively with SQL Server via EF Core with minimal integration friction — a good fit for "keep it simple" POC scope. LTS was chosen over the latest STS release for a longer support window given this may extend beyond a short-lived POC.

## Decision
Build the backend as an ASP.NET Core (.NET 10 LTS) Web API, using EF Core for SQL Server 2022 access.

## Alternatives Considered

| Option | Why not chosen |
|--------|-----------------|
| .NET 8 LTS | Confirmed superseded by .NET 10 LTS per explicit instruction |
| Node.js / Express | No stated preference; would need a separate SQL Server driver/ORM setup with less first-party tooling support |
| Python / FastAPI | No stated preference; weaker native SQL Server tooling than EF Core |
| Java / Spring Boot | No stated preference; heavier setup than needed for a "keep it simple" POC |

## Consequences
- Gains: first-class SQL Server integration via EF Core, strong typing shared conceptually with the Angular/TypeScript frontend (DTOs), consistent with this repo's existing tooling bias, and the longest support window available (LTS).
- Tradeoff: none outstanding — confirmed by explicit user instruction.

## Related
- Architecture section: §3 Components — Workflow API, Workflow Runtime Engine, Workflow Definition Service
- Supersedes: none
