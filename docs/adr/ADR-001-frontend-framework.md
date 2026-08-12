# ADR-001: Frontend Framework — Angular + Angular Material

> Status: ACCEPTED
> Date: 2026-08-12
> Architecture: docs/architecture.md (v1)

## Context
The POC needs a workflow graph editor, a runtime visualization view, and a dashboard — all UI-heavy, form-and-graph-rich screens that benefit from a batteries-included component library and strong TypeScript tooling. The user explicitly requested Angular (latest) with Material UI components, styled to the Shaw and Partners brand guidelines.

## Decision
Build the frontend as an Angular (latest stable) single-page application using Angular Material for UI components.

## Alternatives Considered

| Option | Why not chosen |
|--------|-----------------|
| React / Next.js | Not requested; would require a different component library and theming approach for no POC benefit |
| Vue 3 | Not requested; smaller ecosystem fit for this team's stated preference |
| Plain HTML/CSS/JS | Would slow down building the graph editor, dashboard, and Material-based theming from scratch |

## Consequences
- Gains: rich, consistent component set (Material), strong typing, familiar Angular CLI tooling, straightforward custom theming via Material's theming APIs.
- Tradeoff: Angular Material's default component set does not include a graph/node editor — that still needs a dedicated diagramming library or custom SVG/canvas work, tracked separately in Architecture §3 (Angular Web Client) and not resolved by this ADR.

## Related
- Architecture section: §3 Components — Angular Web Client
- Supersedes: none
