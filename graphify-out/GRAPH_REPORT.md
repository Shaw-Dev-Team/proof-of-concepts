# Graph Report - src  (2026-08-13)

## Corpus Check
- Corpus is ~1,361 words - fits in a single context window. You may not need a graph.

## Summary
- 168 nodes · 171 edges · 12 communities (11 shown, 1 thin omitted)
- Extraction: 99% EXTRACTED · 1% INFERRED · 0% AMBIGUOUS · INFERRED: 1 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- Community 0
- Community 1
- Community 2
- Community 3
- Community 4
- Community 5
- Community 6
- Community 7
- Community 8
- Community 9
- Community 10
- Community 11

## God Nodes (most connected - your core abstractions)
1. `WorkflowPlatform.Api` - 7 edges
2. `frontend` - 7 edges
3. `scripts` - 7 edges
4. `InitialEmpty` - 6 edges
5. `http` - 6 edges
6. `https` - 6 edges
7. `options` - 6 edges
8. `development` - 6 edges
9. `App` - 6 edges
10. `architect` - 5 edges

## Surprising Connections (you probably didn't know these)
- `HTML Bootstrap Page` --references--> `App`  [INFERRED]
  src/frontend/src/index.html → frontend/src/app/app.ts
- `App Root Template` --references--> `App`  [EXTRACTED]
  src/frontend/src/app/app.html → frontend/src/app/app.ts

## Import Cycles
- None detected.

## Communities (12 total, 1 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.09
Nodes (23): @angular/build, @angular/cli, @angular/compiler-cli, angular-eslint, eslint, @eslint/js, devDependencies, @angular/build (+15 more)

### Community 1 - "Community 1"
Cohesion: 0.10
Nodes (21): @angular/cdk, @angular/common, @angular/compiler, @angular/core, @angular/forms, @angular/material, @angular/platform-browser, @angular/router (+13 more)

### Community 2 - "Community 2"
Cohesion: 0.11
Nodes (12): WorkflowPlatformDbContext, ModelBuilder, InitialEmpty, ModelBuilder, WorkflowPlatformDbContextModelSnapshot, WeatherForecast, WorkflowPlatform.Api.Data, WorkflowPlatform.Api.Migrations (+4 more)

### Community 3 - "Community 3"
Cohesion: 0.12
Nodes (17): build, serve, builder, configurations, defaultConfiguration, development, production, buildTarget (+9 more)

### Community 4 - "Community 4"
Cohesion: 0.12
Nodes (17): lint, test, architect, prefix, projectType, root, schematics, sourceRoot (+9 more)

### Community 5 - "Community 5"
Cohesion: 0.13
Nodes (15): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, applicationUrl, commandName (+7 more)

### Community 6 - "Community 6"
Cohesion: 0.14
Nodes (12): cli, packageManager, schematicCollections, newProjectRoot, projects, $schema, version, angular (+4 more)

### Community 7 - "Community 7"
Cohesion: 0.17
Nodes (11): name, packageManager, private, scripts, build, lint, ng, start (+3 more)

### Community 8 - "Community 8"
Cohesion: 0.25
Nodes (6): App Root Template, Component, App, appConfig, routes, HTML Bootstrap Page

### Community 9 - "Community 9"
Cohesion: 0.25
Nodes (7): WorkflowPlatform.Api, net10.0, Microsoft.AspNetCore.OpenApi (10.0.11), Microsoft.EntityFrameworkCore (10.0.11), Microsoft.EntityFrameworkCore.Design (10.0.11), Microsoft.EntityFrameworkCore.SqlServer (10.0.11), Microsoft.NET.Sdk.Web

### Community 10 - "Community 10"
Cohesion: 0.29
Nodes (7): options, assets, browser, inlineStyleLanguage, styles, tsConfig, src/styles.scss

## Knowledge Gaps
- **83 isolated node(s):** `WeatherForecast`, `$schema`, `commandName`, `dotnetRunMessages`, `launchBrowser` (+78 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **1 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `devDependencies` connect `Community 0` to `Community 7`?**
  _High betweenness centrality (0.068) - this node is a cross-community bridge._
- **Why does `architect` connect `Community 4` to `Community 3`?**
  _High betweenness centrality (0.068) - this node is a cross-community bridge._
- **Why does `dependencies` connect `Community 1` to `Community 7`?**
  _High betweenness centrality (0.063) - this node is a cross-community bridge._
- **What connects `WeatherForecast`, `$schema`, `commandName` to the rest of the system?**
  _83 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.08695652173913043 - nodes in this community are weakly interconnected._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.09523809523809523 - nodes in this community are weakly interconnected._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.10952380952380952 - nodes in this community are weakly interconnected._