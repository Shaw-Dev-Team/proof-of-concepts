# Graph Report - src  (2026-08-13)

## Corpus Check
- 40 files · ~4,718 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 245 nodes · 301 edges · 14 communities (13 shown, 1 thin omitted)
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `0ecc3880`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- WorkflowPlatform.Api.Domain
- development
- devDependencies
- frontend
- dependencies
- InitialSchema
- .Create
- http
- WorkflowPlatform.Api.Tests
- package.json
- App
- Frontend
- options
- EnumTests

## God Nodes (most connected - your core abstractions)
1. `WorkflowPlatform.Api.Domain` - 14 edges
2. `WorkflowPlatformDbContext` - 11 edges
3. `Node` - 10 edges
4. `WorkflowDefinition` - 10 edges
5. `WorkflowInstance` - 9 edges
6. `WorkflowPlatform.Api.Tests` - 8 edges
7. `WorkflowPlatform.Api` - 8 edges
8. `NodeConnectionTests` - 7 edges
9. `NodeExecution` - 7 edges
10. `frontend` - 7 edges

## Surprising Connections (you probably didn't know these)
- `Node` --references--> `NodeType`  [EXTRACTED]
  backend/WorkflowPlatform.Api/Domain/Node.cs → backend/WorkflowPlatform.Api/Domain/NodeType.cs
- `NodeExecution` --references--> `NodeExecutionState`  [EXTRACTED]
  backend/WorkflowPlatform.Api/Domain/NodeExecution.cs → backend/WorkflowPlatform.Api/Domain/NodeExecutionState.cs
- `WorkflowInstance` --references--> `WorkflowInstanceStatus`  [EXTRACTED]
  backend/WorkflowPlatform.Api/Domain/WorkflowInstance.cs → backend/WorkflowPlatform.Api/Domain/WorkflowInstanceStatus.cs
- `WorkflowPlatformDbContext` --references--> `Connection`  [EXTRACTED]
  backend/WorkflowPlatform.Api/Data/WorkflowPlatformDbContext.cs → backend/WorkflowPlatform.Api/Domain/Connection.cs
- `WorkflowPlatformDbContext` --references--> `Node`  [EXTRACTED]
  backend/WorkflowPlatform.Api/Data/WorkflowPlatformDbContext.cs → backend/WorkflowPlatform.Api/Domain/Node.cs

## Import Cycles
- None detected.

## Communities (14 total, 1 thin omitted)

### Community 0 - "WorkflowPlatform.Api.Domain"
Cohesion: 0.07
Nodes (27): ModelBuilder, WorkflowPlatformDbContext, Guid, Connection, Guid, ICollection, Node, DateTime (+19 more)

### Community 1 - "development"
Cohesion: 0.08
Nodes (26): build, lint, serve, test, builder, configurations, defaultConfiguration, development (+18 more)

### Community 2 - "devDependencies"
Cohesion: 0.09
Nodes (23): @angular/build, @angular/cli, @angular/compiler-cli, angular-eslint, eslint, @eslint/js, devDependencies, @angular/build (+15 more)

### Community 3 - "frontend"
Cohesion: 0.09
Nodes (20): cli, packageManager, schematicCollections, prefix, projectType, root, schematics, sourceRoot (+12 more)

### Community 4 - "dependencies"
Cohesion: 0.10
Nodes (21): @angular/cdk, @angular/common, @angular/compiler, @angular/core, @angular/forms, @angular/material, @angular/platform-browser, @angular/router (+13 more)

### Community 5 - "InitialSchema"
Cohesion: 0.11
Nodes (11): ModelBuilder, InitialSchema, ModelBuilder, WorkflowPlatformDbContextModelSnapshot, WeatherForecast, TestDbContextFactory, WorkflowPlatform.Api.Data, WorkflowPlatform.Api.Migrations (+3 more)

### Community 6 - ".Create"
Cohesion: 0.23
Nodes (6): Fact, NodeConnectionTests, Fact, WorkflowDefinitionTests, Fact, WorkflowInstanceExecutionTests

### Community 7 - "http"
Cohesion: 0.13
Nodes (15): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, applicationUrl, commandName (+7 more)

### Community 8 - "WorkflowPlatform.Api.Tests"
Cohesion: 0.14
Nodes (14): WorkflowPlatform.Api.Tests, net10.0, WorkflowPlatform.Api, net10.0, Microsoft.AspNetCore.OpenApi (10.0.11), Microsoft.EntityFrameworkCore (10.0.11), Microsoft.EntityFrameworkCore.Design (10.0.11), Microsoft.EntityFrameworkCore.InMemory (10.0.11) (+6 more)

### Community 9 - "package.json"
Cohesion: 0.17
Nodes (11): name, packageManager, private, scripts, build, lint, ng, start (+3 more)

### Community 10 - "App"
Cohesion: 0.33
Nodes (4): Component, App, appConfig, routes

### Community 11 - "Frontend"
Cohesion: 0.25
Nodes (7): Additional Resources, Building, Code scaffolding, Development server, Frontend, Running end-to-end tests, Running unit tests

### Community 12 - "options"
Cohesion: 0.29
Nodes (7): options, assets, browser, inlineStyleLanguage, styles, tsConfig, src/styles.scss

## Knowledge Gaps
- **92 isolated node(s):** `net10.0`, `Microsoft.EntityFrameworkCore.InMemory (10.0.11)`, `Microsoft.NET.Test.Sdk (17.12.0)`, `xunit (2.9.2)`, `xunit.runner.visualstudio (2.8.2)` (+87 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **1 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `WorkflowPlatformDbContext` connect `WorkflowPlatform.Api.Domain` to `InitialSchema`, `.Create`?**
  _High betweenness centrality (0.043) - this node is a cross-community bridge._
- **Why does `devDependencies` connect `devDependencies` to `package.json`?**
  _High betweenness centrality (0.032) - this node is a cross-community bridge._
- **What connects `net10.0`, `Microsoft.EntityFrameworkCore.InMemory (10.0.11)`, `Microsoft.NET.Test.Sdk (17.12.0)` to the rest of the system?**
  _92 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `WorkflowPlatform.Api.Domain` be split into smaller, more focused modules?**
  _Cohesion score 0.07195121951219512 - nodes in this community are weakly interconnected._
- **Should `development` be split into smaller, more focused modules?**
  _Cohesion score 0.08307692307692308 - nodes in this community are weakly interconnected._
- **Should `devDependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.08695652173913043 - nodes in this community are weakly interconnected._
- **Should `frontend` be split into smaller, more focused modules?**
  _Cohesion score 0.09090909090909091 - nodes in this community are weakly interconnected._