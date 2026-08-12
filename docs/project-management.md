# Project Management: Workflow Visualization and Execution Platform POC

> Status: ACTIVE
> Tranche: v1
> Version: v1
> Last updated: 2026-08-12
> PRD: docs/prd.md (built against v5)
> Architecture: docs/architecture.md (built against v5)

## Phases & Milestones

| Phase | Goal | Status |
|-------|------|--------|
| Phase 0 | Project scaffolding (Angular app, ASP.NET Core API, SQL Server/EF Core setup) | Planned |
| Phase 1 | Domain & data layer (entity schema, EF Core migrations) | Planned |
| Phase 2 | Workflow Definition Service & API (CRUD + versioning) | Planned |
| Phase 3 | Task Handler abstraction (interfaces + mock handlers) | Planned |
| Phase 4 | Workflow Runtime Engine (instance lifecycle, node evaluation) | Planned |
| Phase 5 | Angular frontend (persona selection, designer, runtime viewer, dashboard) | Planned |
| Phase 6 | Branding & theming (Angular Material theme, licensed fonts) | Planned |
| Phase 7 | Demo scenarios (end-to-end validation workflows) | Planned |

## Feature Backlog

| ID | Feature | Phase | Priority | Status | Acceptance Criteria |
|----|---------|-------|----------|--------|---------------------|
| F-001 | Domain/data schema + EF Core migrations against SQL Server 2022 | 1 | Must | Planned | All entities from Architecture §4 (WorkflowDefinition, Node, Connection, WorkflowInstance, NodeExecution, TaskHandlerReference) exist as EF Core-mapped tables; migrations apply cleanly to a fresh SQL Server 2022 instance |
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
| PM-001 | Confirm SQL Server 2022 connection/environment details for Phase 1 migrations | Mo | Before Phase 1 starts |

## Version History
| Version | Date | Change | Triggered By |
|---------|------|--------|---------------|
| v1 | 2026-08-12 | Initial draft | — |
