# Test Runbook: F-001 — Domain/Data Schema + EF Core Migrations (SQL Server 2022)

> Feature: Domain/data schema + EF Core migrations against SQL Server 2022
> Phase: 0
> Status: Done

## Automated Tests

Project: [src/backend/WorkflowPlatform.Api.Tests/](../../src/backend/WorkflowPlatform.Api.Tests/) (xUnit, EF Core in-memory provider — one fresh database per test via [TestDbContextFactory.cs](../../src/backend/WorkflowPlatform.Api.Tests/TestDbContextFactory.cs))

```powershell
cd src/backend
dotnet test
```

- Expected: 13 tests pass.

### Entity CRUD, nullability, and independent-version coexistence — [WorkflowDefinitionTests.cs](../../src/backend/WorkflowPlatform.Api.Tests/WorkflowDefinitionTests.cs)

- `WorkflowDefinition_CanBePersistedAndRetrieved_WithZeroNodes` — a zero-node `WorkflowDefinition` persists and reloads with empty `Nodes`/`Connections` collections.
- `WorkflowDefinition_MissingRequiredName_ThrowsOnSave` — a null `Name` throws `DbUpdateException` on save (required-field enforcement).
- `TwoDefinitions_SameName_DifferentVersions_CoexistIndependently` — two `WorkflowDefinition` rows sharing a `Name` at different `Version` values both persist and are independently retrievable.

### Relationship loading, `ConditionExpression` nullability, `TaskHandlerReference` — [NodeConnectionTests.cs](../../src/backend/WorkflowPlatform.Api.Tests/NodeConnectionTests.cs)

- `WorkflowDefinition_LoadsItsNodesAndConnections` — `WorkflowDefinition.Nodes`/`Connections` load correctly via `Include`.
- `Connection_ConditionExpression_NullForNonBranchingConnection` — `ConditionExpression` is `null` for a non-branching connection.
- `Connection_ConditionExpression_PopulatedForBranchingConnection` — `ConditionExpression` is populated for a branching connection.
- `TaskHandlerReference_AssociatesWithTaskNode` — a `TaskHandlerReference` associates correctly with its `Task`-type `Node`.
- `TaskHandlerReference_MissingRequiredHandlerType_ThrowsOnSave` — a null `HandlerType` throws `DbUpdateException` on save.

### `WorkflowInstance` execution history and version snapshotting — [WorkflowInstanceExecutionTests.cs](../../src/backend/WorkflowPlatform.Api.Tests/WorkflowInstanceExecutionTests.cs)

- `WorkflowInstance_LoadsItsExecutionHistory` — `WorkflowInstance.ExecutionHistory` loads its `NodeExecution` records via `Include`.
- `WorkflowInstance_DefinitionVersion_IsSnapshotNotLiveReference` — `WorkflowInstance.DefinitionVersion` remains the version captured at instance creation, unaffected by a later `WorkflowDefinition` version being published.
- `NodeExecution_EvaluationOutcome_NullUntilEvaluated` — `NodeExecution.EvaluationOutcome` is `null` before a condition node has been evaluated.

### Enum completeness — [EnumTests.cs](../../src/backend/WorkflowPlatform.Api.Tests/EnumTests.cs)

- `NodeExecutionState_ContainsExactlyTheLifecycleDiagramStates` — `NodeExecutionState` contains exactly `Pending, Ready, Running, Completed, Failed, Skipped`, matching [workflow-node-lifecycle.mmd](../diagrams/mmd/workflow-node-lifecycle.mmd).
- `WorkflowInstanceStatus_ContainsExactlyTheLifecycleDiagramStates` — `WorkflowInstanceStatus` contains exactly `Draft, Running, Completed, Failed, Paused, Cancelled`, matching [workflow-instance-lifecycle.mmd](../diagrams/mmd/workflow-instance-lifecycle.mmd).

## Manual Verification

### 1. Migration applies cleanly against a real LocalDB instance and creates all domain tables

The automated suite above runs entirely against the EF Core in-memory provider — it does not confirm the migration itself applies correctly to a real relational engine. Verify that manually:

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "DROP DATABASE IF EXISTS WorkflowPlatformDb;" -C
cd src/backend\WorkflowPlatform.Api
dotnet ef database update
```

- Expected: the migration applies cleanly with no errors.

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -d WorkflowPlatformDb -Q "SELECT name FROM sys.tables ORDER BY name;" -C
```

- Expected: exactly 7 tables listed — the 6 domain tables (`WorkflowDefinitions`, `Nodes`, `Connections`, `WorkflowInstances`, `NodeExecutions`, `TaskHandlerReferences`) plus the `WorkflowInstanceCurrentNodes` join table (backing `WorkflowInstance.CurrentNodes`, a many-to-many with no dedicated entity).

### 2. Re-verify against a full SQL Server 2022 Database Engine when available

- A full SQL Server 2022 Database Engine instance is not installed on the development machine as of this writing — only LocalDB has been verified (see PMBook PM-001). Re-run Step 1 against a real SQL Server 2022 instance once one is available, and update this note accordingly.

## Notes

- Automated tests use the EF Core in-memory provider exclusively; they verify entity behavior (CRUD, nullability, relationship loading, enum completeness) but not real SQL Server schema generation — hence Step 1 above as a required manual complement.
- Do not report Step 2 as passing until a real SQL Server 2022 instance (not LocalDB) is available and re-verified.
