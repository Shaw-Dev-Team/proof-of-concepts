# ADR-003: Database — SQL Server 2022 via EF Core

> Status: ACCEPTED
> Date: 2026-08-12
> Architecture: docs/architecture.md (v1)

## Context
The user explicitly specified SQL Server 2022 as the database for this POC.

## Decision
Persist workflow definitions, instances, and execution history in SQL Server 2022, accessed through EF Core from the ASP.NET Core backend.

## Alternatives Considered

| Option | Why not chosen |
|--------|-----------------|
| PostgreSQL | Not requested |
| Document store (e.g. Cosmos DB / MongoDB) | Not requested; relational model fits the versioned-definition + instance-history shape described in the PRD and prior domain-model notes |

## Consequences
- Gains: mature relational modeling for versioned WorkflowDefinition, Node, Connection, WorkflowInstance, and NodeExecution/ExecutionHistory records; strong tooling via EF Core migrations.
- Tradeoff: node graphs and execution history are naturally tree/graph-shaped; the relational schema needs deliberate normalization (e.g. adjacency-style Node/Connection tables) rather than a document-per-graph approach — this is expected and acceptable for POC scope.

## Related
- Architecture section: §3 Components — Data Store
- Supersedes: none
