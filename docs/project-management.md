# Project Management: Workflow Visualization and Execution Platform POC

> Status: ACTIVE
> Tranche: v1
> Version: v1
> Last updated: 2026-08-13
> PRD: docs/prd.md (built against v5)
> Architecture: docs/architecture.md (built against v5)

## Phases & Milestones

| Phase | Goal | Status |
|-------|------|--------|
| Phase 0 | Project scaffolding (Angular app, ASP.NET Core API, SQL Server/EF Core setup) | Done |
| Phase 1 | Domain & data layer (entity schema, EF Core migrations) | Done |
| Phase 2 | Workflow Definition Service & API (CRUD + versioning) | Planned |
| Phase 3 | Task Handler abstraction (interfaces + mock handlers) | Planned |
| Phase 4 | Workflow Runtime Engine (instance lifecycle, node evaluation) | Planned |
| Phase 5 | Angular frontend (persona selection, designer, runtime viewer, dashboard) | Planned |
| Phase 6 | Branding & theming (Angular Material theme, licensed fonts) | Planned |
| Phase 7 | Demo scenarios (end-to-end validation workflows) | Planned |

## Feature Backlog

| ID | Feature | Phase | Priority | Status | Acceptance Criteria |
|----|---------|-------|----------|--------|---------------------|
| F-001 | Domain/data schema + EF Core migrations against SQL Server 2022 | 1 | Must | Done | All entities from Architecture §4 (WorkflowDefinition, Node, Connection, WorkflowInstance, NodeExecution, TaskHandlerReference) exist as EF Core-mapped tables; migrations apply cleanly to a fresh SQL Server 2022 instance — verified against `(localdb)\MSSQLLocalDB` (a full SQL Server 2022 engine is not installed on this machine; LocalDB used for local dev, consistent with F-015/PM-001) |
| F-002 | Workflow Definition CRUD + versioning API/service | 2 | Must | Planned | Satisfies FR-002; definitions can be created, retrieved, updated, and versioned independently of any running instance |
| F-003 | Task Handler abstraction (`ITaskHandler` / `ITaskCompletionCallback`) + mock handlers | 3 | Must | Planned | Satisfies FR-005 and ADR-004; at least SendEmail, CreateInvoice, ValidateRecord, and a HumanApproval-style stub implement the interface |
| F-004 | Workflow Runtime Engine (instance lifecycle, node evaluation, task invocation) | 4 | Must | Planned | Satisfies FR-004 and Architecture §5 Data Flow; instance moves through the Workflow Instance Lifecycle (§4) and node-level states (§5) correctly for Condition, Switch, Loop, Parallel Split/Merge, Wait/Pause, and Manual Approval nodes |
| F-005 | Persona-selection entry screen (Workflow Designer selectable; Process Owner/Operations Analyst greyed out) | 5 | Must | Planned | Satisfies FR-001 |
| F-006 | Workflow Designer graph editor (create/save node graphs) | 5 | Must | Planned | Satisfies FR-002 (UI side); depends on F-002 |
| F-007 | Runtime visualization view (node status, current position, completed/skipped path) | 5 | Must | Planned | Satisfies FR-004 (UI side); depends on F-004 |
| F-008 | Dashboard (status counts, filterable instance list, per-instance drill-in) | 5 | Should | Planned | Satisfies FR-007; depends on F-004 |
| F-009 | Angular Material brand theme (Shaw Orange palette, licensed Catalog Regular/Helvetica Neue) | 6 | Must | Planned | Satisfies NFR-006 and ADR-006 |
| F-010 | Demo scenario: Customer Onboarding workflow | 7 | Should | Planned | End-to-end run validates PRD Success Metrics using a Condition + Manual Approval branch |
| F-011 | Demo scenario: Invoice Processing workflow | 7 | Should | Planned | End-to-end run validates Parallel Split/Merge behavior |
| F-012 | Demo scenario: Employee Offboarding workflow | 7 | Should | Planned | End-to-end run validates Wait/Pause behavior |
| F-013 | Angular workspace scaffolding (Angular CLI latest, standalone app, Angular Material installed) | 0 | Must | Done | `ng new` workspace builds and serves; Angular Material schematic installed; base folder structure in place per ADR-001 |
| F-014 | ASP.NET Core Web API scaffolding (.NET 10 LTS solution/project, EF Core + SQL Server provider references) | 0 | Must | Done | Solution builds (`dotnet build`) and runs; EF Core + Microsoft.EntityFrameworkCore.SqlServer package references added per ADR-002/ADR-003 |
| F-015 | SQL Server 2022 / EF Core connection setup (DbContext skeleton, connection string configuration) | 0 | Must | Done | Empty `DbContext` registered in DI; connection string configurable via appsettings/environment; database creation and pending migrations are applied automatically on startup (`Database.Migrate()` in `Program.cs`) — verified for real against `(localdb)\MSSQLLocalDB`; a full SQL Server 2022 engine is not installed on this machine (LocalDB used for local dev instead) |

(IDs must be stable — never renumber once assigned)

## Dependencies

| Item | Depends On | Blocks | Notes |
|------|-----------|--------|-------|
| F-002 | F-001 | F-006 | Definition service needs the schema in place |
| F-003 | F-001 | F-004 | Task handler interface needs entity context (TaskHandlerReference) |
| F-004 | F-001, F-003 | F-007, F-008, F-010, F-011, F-012 | Runtime engine needs both schema and task handler abstraction |
| F-006 | F-002 | F-010, F-011, F-012 | Designer UI needs a working definition API |
| F-007 | F-004 | F-010, F-011, F-012 | Runtime viewer needs a working engine |
| F-009 | — | F-005, F-006, F-007, F-008 | Theme should land before UI screens are finalized visually |
| F-010, F-011, F-012 | F-006, F-007 | — | Demo scenarios need the full designer + runtime path working |
| F-001 | F-014, F-015 | F-002, F-003 | Domain schema needs the backend project + EF Core/SQL Server setup in place first |
| F-002 | F-001, F-014 | F-006 | (updated) Definition service needs the schema and the backend API project scaffold |
| F-006 | F-002, F-013 | F-010, F-011, F-012 | (updated) Designer UI also needs the Angular workspace scaffold, not just the definition API |

## Out of Scope (confirmed in PRD)

- Full enterprise workflow orchestration product (this tranche)
- Real business system integration / live production task execution
- Advanced automation, AI, or plugin marketplaces
- Large-graph / high-volume production optimization
- Polished low-code editor (basic editor/visualization acceptable)
- Nested workflows or sub-processes
- Task retries, timeouts, or SLA enforcement
- Distributed execution engine, event streaming, or multi-tenant execution
- BPMN import/export
- Authentication / login (PRD Q1)
- See also PRD "Future Considerations" — recorded for continuity, not planned for any phase in this Tranche

## Open Items

| ID | Item | Owner | Due |
|----|------|-------|-----|
| PM-001 | ~~No SQL Server 2022 Database Engine is actually installed on this machine~~ **RESOLVED** — `src/backend/WorkflowPlatform.Api/appsettings.Development.json` now targets `(localdb)\MSSQLLocalDB` instead of `localhost`. `dotnet ef database update` verified working for real: creates `WorkflowPlatformDb.mdf`/`.ldf` under the user profile and applies migrations. A full SQL Server 2022 engine is still not installed, but LocalDB is sufficient for local development; revisit only if a non-dev environment needs a real SQL Server 2022 instance. | Mo | Resolved 2026-08-13 |

## Version History
| Version | Date | Change | Triggered By |
|---------|------|--------|---------------|
| v1 | 2026-08-12 | Initial draft | — |
| v2 | 2026-08-12 | Added Phase 0 features F-013/F-014/F-015 (Angular, ASP.NET Core, EF Core/SQL Server scaffolding) — Phase 0 had no backlog entries; updated dependencies for F-001/F-002/F-006 accordingly | Orchestrator Phase 0 init |
| v3 | 2026-08-13 | Marked Phase 0 Done in Phases & Milestones (F-013/F-014/F-015 confirmed Done in Feature Backlog); PM-001 left open | Finalizer PMBook Update |
| v4 | 2026-08-13 | Moved `frontend/`/`backend/` → `src/frontend/`/`src/backend/`; resolved PM-001 via LocalDB connection string (verified real `dotnet ef database update`); replaced fabricated Graphify output with a real pipeline run | Post-Phase-0 corrective fix |
| v5 | 2026-08-13 | Deleted stale duplicate `backend/`/`frontend/` folders left behind at repo root by the prior "move" (which had copied to `src/` without actually removing the originals — confirmed via direct file comparison, root copies were strictly older/incomplete); removed a stray `src/graphify-out/cache/` artifact; re-verified both projects build from `src/` | Post-Phase-0 corrective fix |
| v6 | 2026-08-13 | Added automatic EF Core migration-on-startup (`Database.Migrate()` in `Program.cs`) so `dotnet run`/`make up-backend` creates the database and applies migrations with no manual step; verified for real by dropping and recreating `WorkflowPlatformDb` against LocalDB; updated F-015 acceptance criteria and test runbook | Post-Phase-0 enhancement |
| v7 | 2026-08-13 | Marked F-001 Done (EF Core entity schema + `InitialSchema` migration, verified against LocalDB) and Phase 1 Done | Orchestrator Feature Loop — F-001 Reviewer PASS |
| v8 | 2026-08-13 | Integration verification confirmed for F-001: all entities (WorkflowDefinition, Node, Connection, WorkflowInstance, NodeExecution, TaskHandlerReference) exist as `DbSet<T>`-mapped tables, migration re-verified against LocalDB, no code fixes required. A documentation-only correction (Architecture §4 field descriptions, navigation-collection shape) was applied directly to `docs/architecture.md` (its own v6) and is not duplicated here. Phase 2 (Workflow Definition Service & API) confirmed as the next Planned phase; no Open Items are affected by this work; not a Tranche boundary — Phases 2–7 remain Planned | Finalizer PMBook Update — F-001 Integration PASS |
