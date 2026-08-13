# Graph Report - .  (2026-08-13)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 204 nodes · 430 edges · 13 communities (9 shown, 4 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 25 edges (avg confidence: 0.8)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `930c4f0f`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- WorkflowPlatform.Api.Domain
- WorkflowPlatform.Api.Data
- IWorkflowDefinitionService
- InitialSchema
- .Create
- .CreateVersionAsync
- http
- WorkflowPlatform.Api.Tests
- CreateWorkflowDefinitionRequest
- NodeConnectionTests
- WorkflowInstanceExecutionTests
- EnumTests
- WorkflowDefinitionTests

## God Nodes (most connected - your core abstractions)
1. `WorkflowPlatform.Api.Domain` - 18 edges
2. `IWorkflowDefinitionService` - 14 edges
3. `WorkflowDefinitionServiceTests` - 14 edges
4. `WorkflowDefinitionService` - 14 edges
5. `WorkflowDefinition` - 13 edges
6. `CreateWorkflowDefinitionRequest` - 13 edges
7. `WorkflowPlatformDbContext` - 12 edges
8. `Node` - 10 edges
9. `WorkflowDefinitionResponse` - 10 edges
10. `WorkflowInstance` - 9 edges

## Surprising Connections (you probably didn't know these)
- `WorkflowPlatform.Api.Tests` --references--> `net10.0`  [EXTRACTED]
  WorkflowPlatform.Api.Tests/WorkflowPlatform.Api.Tests.csproj → WorkflowPlatform.Api/WorkflowPlatform.Api.csproj
- `WorkflowDefinitionService` --references--> `WorkflowPlatformDbContext`  [EXTRACTED]
  WorkflowPlatform.Api/Services/WorkflowDefinitionService.cs → WorkflowPlatform.Api/Data/WorkflowPlatformDbContext.cs
- `NodeExecution` --references--> `NodeExecutionState`  [EXTRACTED]
  WorkflowPlatform.Api/Domain/NodeExecution.cs → WorkflowPlatform.Api/Domain/NodeExecutionState.cs
- `WorkflowInstance` --references--> `WorkflowInstanceStatus`  [EXTRACTED]
  WorkflowPlatform.Api/Domain/WorkflowInstance.cs → WorkflowPlatform.Api/Domain/WorkflowInstanceStatus.cs
- `WorkflowDefinitionService` --implements--> `IWorkflowDefinitionService`  [EXTRACTED]
  WorkflowPlatform.Api/Services/WorkflowDefinitionService.cs → WorkflowPlatform.Api/Services/IWorkflowDefinitionService.cs

## Import Cycles
- None detected.

## Communities (13 total, 4 thin omitted)

### Community 0 - "WorkflowPlatform.Api.Domain"
Cohesion: 0.07
Nodes (27): WorkflowPlatform.Api.Domain, WorkflowPlatform.Api.Tests, DbContext, DbSet, ModelBuilder, WorkflowPlatformDbContext, Guid, Connection (+19 more)

### Community 1 - "WorkflowPlatform.Api.Data"
Cohesion: 0.18
Nodes (9): WorkflowPlatform.Api.Contracts, WorkflowPlatform.Api.Data, WorkflowPlatform.Api.Services, WorkflowPlatform.Api.Endpoints, Exception, IDictionary, WorkflowDefinitionConflictException, WorkflowDefinitionValidationException (+1 more)

### Community 2 - "IWorkflowDefinitionService"
Cohesion: 0.20
Nodes (12): IEndpointRouteBuilder, IResult, RouteGroupBuilder, CancellationToken, Guid, Task, WorkflowDefinitionEndpoints, CancellationToken (+4 more)

### Community 3 - "InitialSchema"
Cohesion: 0.11
Nodes (11): WorkflowPlatform.Api.Migrations, Migration, ModelSnapshot, MigrationBuilder, ModelBuilder, InitialSchema, MigrationBuilder, ModelBuilder (+3 more)

### Community 4 - ".Create"
Cohesion: 0.44
Nodes (3): Fact, Task, WorkflowDefinitionServiceTests

### Community 5 - ".CreateVersionAsync"
Cohesion: 0.22
Nodes (9): ConnectionResponse, NodeResponse, WorkflowDefinitionResponse, WorkflowDefinitionSummaryResponse, CancellationToken, Guid, IReadOnlyList, Task (+1 more)

### Community 6 - "http"
Cohesion: 0.13
Nodes (15): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, applicationUrl, commandName (+7 more)

### Community 7 - "WorkflowPlatform.Api.Tests"
Cohesion: 0.15
Nodes (14): Microsoft.AspNetCore.OpenApi (10.0.11), Microsoft.EntityFrameworkCore (10.0.11), Microsoft.EntityFrameworkCore.Design (10.0.11), Microsoft.EntityFrameworkCore.InMemory (10.0.11), Microsoft.EntityFrameworkCore.Sqlite (10.0.11), Microsoft.EntityFrameworkCore.SqlServer (10.0.11), Microsoft.NET.Test.Sdk (17.12.0), xunit (2.9.2) (+6 more)

### Community 8 - "CreateWorkflowDefinitionRequest"
Cohesion: 0.53
Nodes (5): List, Guid, CreateConnectionRequest, CreateNodeRequest, CreateWorkflowDefinitionRequest

## Knowledge Gaps
- **22 isolated node(s):** `ConnectionResponse`, `NodeResponse`, `applicationUrl`, `commandName`, `dotnetRunMessages` (+17 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **4 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `WorkflowPlatform.Api.Data` connect `WorkflowPlatform.Api.Data` to `WorkflowPlatform.Api.Domain`, `InitialSchema`?**
  _High betweenness centrality (0.180) - this node is a cross-community bridge._
- **Why does `WorkflowPlatformDbContext` connect `WorkflowPlatform.Api.Domain` to `.Create`, `.CreateVersionAsync`?**
  _High betweenness centrality (0.146) - this node is a cross-community bridge._
- **Why does `WorkflowPlatform.Api.Domain` connect `WorkflowPlatform.Api.Domain` to `CreateWorkflowDefinitionRequest`, `WorkflowPlatform.Api.Data`, `.CreateVersionAsync`?**
  _High betweenness centrality (0.142) - this node is a cross-community bridge._
- **What connects `ConnectionResponse`, `NodeResponse`, `applicationUrl` to the rest of the system?**
  _22 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `WorkflowPlatform.Api.Domain` be split into smaller, more focused modules?**
  _Cohesion score 0.07084785133565621 - nodes in this community are weakly interconnected._
- **Should `InitialSchema` be split into smaller, more focused modules?**
  _Cohesion score 0.1067193675889328 - nodes in this community are weakly interconnected._
- **Should `http` be split into smaller, more focused modules?**
  _Cohesion score 0.13333333333333333 - nodes in this community are weakly interconnected._