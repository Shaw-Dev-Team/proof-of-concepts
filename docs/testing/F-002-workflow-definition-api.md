# Test Runbook: F-002 — Workflow Definition CRUD + Versioning API/Service

> Feature: Workflow Definition CRUD + versioning API/service (FR-002)
> Phase: 0
> Status: Done

## Automated Tests

Project: [src/backend/WorkflowPlatform.Api.Tests/](../../src/backend/WorkflowPlatform.Api.Tests/) (xUnit, EF Core in-memory provider unless noted otherwise — one fresh database per test via [TestDbContextFactory.cs](../../src/backend/WorkflowPlatform.Api.Tests/TestDbContextFactory.cs))

```powershell
cd src/backend
dotnet test
```

- Expected: 25 tests pass (13 from F-001 + 12 from this feature).

### `WorkflowDefinitionService` CRUD + versioning — [WorkflowDefinitionServiceTests.cs](../../src/backend/WorkflowPlatform.Api.Tests/WorkflowDefinitionServiceTests.cs)

- `CreateAsync_WithNodesAndConnections_PersistsAllThreeEntityTypes` — creating a definition with nodes + connections persists all three entity types (`WorkflowDefinition`, `Node`, `Connection`) transactionally as `Version` 1.
- `CreateAsync_WithZeroNodesAndConnections_PersistsAsEmptyDraft` — a definition with zero nodes/connections is a valid empty draft.
- `CreateVersionAsync_ForExistingDefinition_IncrementsVersionAndLeavesPriorVersionUnchanged` — creating a new version increments `Version` and leaves the prior version's row (including its nodes) byte-for-byte unchanged.
- `CreateVersionAsync_ReusingAPriorVersionsNodeId_SucceedsAsDistinctRowPerVersion` — creating a new version that reuses a prior version's `Node` ID succeeds as a distinct row-set per version, rather than colliding with or mutating the prior version's row. This is the defect found and fixed during Integration's retry pass via the `Node` composite-key (`NodeId`, `WorkflowDefinitionId`) schema change — see [Node.cs](../../src/backend/WorkflowPlatform.Api/Domain/Node.cs).
- `CreateVersionAsync_ForNameThatDoesNotExistYet_BehavesAsVersionOneCreation` — version creation for a nonexistent definition name behaves as version-1 creation.
- `GetLatestByNameAsync_ReturnsHighestVersionRow` — retrieving the latest version by name returns the highest `Version` row.
- `ListVersionsByNameAsync_ReturnsAllVersionsInVersionOrder` — listing all versions by name returns them in version order.
- `ListDistinctDefinitionsAsync_ReturnsOneRowPerNameAtLatestVersion` — listing distinct definitions returns one row per distinct `Name`, at its latest version.
- `CreateAsync_MissingName_ThrowsValidationException` — a missing/whitespace `Name` throws `WorkflowDefinitionValidationException` with a `Name` error key.
- `CreateAsync_MissingCreatedBy_ThrowsValidationException` — a missing/empty `CreatedBy` throws `WorkflowDefinitionValidationException` with a `CreatedBy` error key.
- `CreateAsync_ConnectionReferencesUnknownNode_ThrowsValidationException` — a `Connection` referencing a node ID not present in the same payload's `Nodes` throws `WorkflowDefinitionValidationException`.
- `ConcurrentSameNameAndVersion_ViolatesUniqueIndex` — two rows sharing `(Name, Version)` cannot both persist; the unique index configured in `WorkflowPlatformDbContext.OnModelCreating` blocks the second `SaveChanges` with `DbUpdateException`. Uses the SQLite provider (not in-memory) specifically because EF Core's in-memory provider doesn't enforce secondary unique indexes.

## Manual Verification

Run these against the live API. Use the [WorkflowPlatform.Api.http](../../src/backend/WorkflowPlatform.Api/WorkflowPlatform.Api.http) file in VS Code/Rider, or the equivalent `curl` commands below.

```powershell
cd src/backend\WorkflowPlatform.Api
dotnet run
```

- Note the listening URL from the startup log (e.g. `http://localhost:5175`); substitute it for `<host>` below.

### 1. Create a definition with nodes + connections (transactional persistence)

```powershell
curl -X POST http://<host>/api/workflow-definitions `
  -H "Content-Type: application/json" `
  -d '{
    "name": "Onboarding",
    "createdBy": "manual-tester",
    "nodes": [
      { "id": "11111111-1111-1111-1111-111111111111", "type": "Start", "name": "Start" },
      { "id": "22222222-2222-2222-2222-222222222222", "type": "End", "name": "End" }
    ],
    "connections": [
      { "sourceNodeId": "11111111-1111-1111-1111-111111111111", "targetNodeId": "22222222-2222-2222-2222-222222222222" }
    ]
  }'
```

- Expected: `201 Created`, body has `version: 1`, `nodes` with 2 entries, `connections` with 1 entry referencing both node IDs.

### 2. Create a zero-node/zero-connection definition (valid empty draft)

```powershell
curl -X POST http://<host>/api/workflow-definitions `
  -H "Content-Type: application/json" `
  -d '{ "name": "Empty Draft", "createdBy": "manual-tester" }'
```

- Expected: `201 Created`, body has `version: 1`, `nodes: []`, `connections: []`.

### 3. Validation failure — missing `Name`

```powershell
curl -X POST http://<host>/api/workflow-definitions `
  -H "Content-Type: application/json" `
  -d '{ "name": "", "createdBy": "manual-tester" }'
```

- Expected: `400 Bad Request` (`ValidationProblem`) with an error entry keyed `Name`.

### 4. Validation failure — missing `CreatedBy`

```powershell
curl -X POST http://<host>/api/workflow-definitions `
  -H "Content-Type: application/json" `
  -d '{ "name": "No Creator", "createdBy": "" }'
```

- Expected: `400 Bad Request` (`ValidationProblem`) with an error entry keyed `CreatedBy`.

### 5. Validation failure — Connection references an unknown node

```powershell
curl -X POST http://<host>/api/workflow-definitions `
  -H "Content-Type: application/json" `
  -d '{
    "name": "Bad Connection",
    "createdBy": "manual-tester",
    "nodes": [ { "id": "11111111-1111-1111-1111-111111111111", "type": "Start", "name": "Start" } ],
    "connections": [
      { "sourceNodeId": "11111111-1111-1111-1111-111111111111", "targetNodeId": "99999999-9999-9999-9999-999999999999" }
    ]
  }'
```

- Expected: `400 Bad Request` (`ValidationProblem`) with an error entry keyed `Connections[0].TargetNodeId` (unknown node).

### 6. Create a new version — increments `Version`, prior version unchanged

Using the `Onboarding` definition created in Step 1:

```powershell
curl -X POST http://<host>/api/workflow-definitions/by-name/Onboarding/versions `
  -H "Content-Type: application/json" `
  -d '{ "name": "Onboarding", "createdBy": "manual-tester", "description": "v2" }'
```

- Expected: `201 Created`, body has `version: 2`, a `definitionId` different from the v1 response's.

```powershell
curl http://<host>/api/workflow-definitions/<v1-definitionId>
```

- Expected: the v1 row is returned unchanged — still `version: 1`, still its original 2 nodes and 1 connection, `description` unaffected by the v2 request.

### 7. Version creation for a nonexistent definition name behaves as version-1 creation

```powershell
curl -X POST http://<host>/api/workflow-definitions/by-name/Brand%20New/versions `
  -H "Content-Type: application/json" `
  -d '{ "name": "Brand New", "createdBy": "manual-tester" }'
```

- Expected: `201 Created`, body has `version: 1` (no prior version existed, so this behaves identically to `POST /api/workflow-definitions`).

### 8. Reusing a prior version's node ID across versions (regression check — previously a real defect)

This is the scenario that failed once during Integration and is now fixed via the `Node` composite-key (`NodeId`, `WorkflowDefinitionId`) schema change. Re-verify it explicitly:

```powershell
curl -X POST http://<host>/api/workflow-definitions `
  -H "Content-Type: application/json" `
  -d '{
    "name": "Reuse Test",
    "createdBy": "manual-tester",
    "nodes": [ { "id": "33333333-3333-3333-3333-333333333333", "type": "Start", "name": "Start v1" } ]
  }'
```

- Expected: `201 Created`, `version: 1`, node `id` is `33333333-3333-3333-3333-333333333333`, node `name` is `Start v1`.

```powershell
curl -X POST http://<host>/api/workflow-definitions/by-name/Reuse%20Test/versions `
  -H "Content-Type: application/json" `
  -d '{
    "name": "Reuse Test",
    "createdBy": "manual-tester",
    "nodes": [ { "id": "33333333-3333-3333-3333-333333333333", "type": "Start", "name": "Start v2" } ]
  }'
```

- Expected: `201 Created` (not a conflict/error), `version: 2`, node `id` is still `33333333-3333-3333-3333-333333333333`, node `name` is `Start v2`.
- Confirm both versions independently: `GET /api/workflow-definitions/by-name/Reuse%20Test/versions` returns both `version: 1` (node name `Start v1`) and `version: 2` (node name `Start v2`) — the shared node ID never collided or overwrote the other version's row.

### 9. Retrieve the latest version by name

```powershell
curl http://<host>/api/workflow-definitions/by-name/Onboarding
```

- Expected: `200 OK`, returns the highest-`Version` row for `Onboarding` (`version: 2`, from Step 6).

### 10. List all versions by name, in version order

```powershell
curl http://<host>/api/workflow-definitions/by-name/Onboarding/versions
```

- Expected: `200 OK`, a JSON array with entries for `version: 1` then `version: 2`, in that order.

### 11. List distinct definitions (one row per name, latest version)

```powershell
curl http://<host>/api/workflow-definitions/
```

- Expected: `200 OK`, exactly one entry per distinct `name` created above (`Onboarding`, `Empty Draft`, `Brand New`, `Reuse Test`), each showing its latest `version` (e.g. `Onboarding` shows `version: 2`, not `1`).

### 12. Concurrent version creation for the same name is blocked

- Not practically triggerable via two manual `curl` calls (the race window is too small to hit reliably by hand) — this is covered by the automated `ConcurrentSameNameAndVersion_ViolatesUniqueIndex` test against SQLite, which exercises the real unique index on `(Name, Version)`. As a manual sanity check only, re-issuing the exact Step 6 request a second time (same name, causing the service to recompute the same next version if run concurrently) should never result in two `version: 2` rows for `Onboarding` — confirm via Step 10 that versions remain sequential with no duplicates.

## Notes

- Steps 1–11 are fully verifiable today against the running API.
- Step 12's true concurrency scenario is only meaningfully exercised by the automated SQLite-backed test, since EF Core's in-memory provider (used by the rest of the automated suite) does not enforce secondary unique indexes.
- The node-ID-reuse case (Step 8) is a regression check: it previously failed before the `Node` composite-key schema change and must not be reported as passing without re-running it after any future change to `Node`'s key configuration.
