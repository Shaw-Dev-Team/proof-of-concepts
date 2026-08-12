# AGENTS.md

## Purpose
This repository is a small proof-of-concepts workspace. It currently contains documentation files and a workflow platform PRD draft, rather than a complete application codebase.

## What agents should know
- The repo root contains `README.md`, a `.github` folder (Copilot instructions/skills/agents config), a `.vscode` folder, and a `docs` folder created for triage artifacts.
- The only substantive content at the moment is a PRD draft for a workflow visualization and execution platform POC in `docs/prd.md`, plus an earlier workflow platform PRD in `docs/workflow-platform-prd.md`, and diagram sources/renders under `docs/diagrams/`.
- There is no detected build, test, or source code project file in this workspace today.
- If asked to implement features, first confirm whether the user wants to keep work in docs only or whether they want to add code/project structure.

## Recommended agent behavior
- Do not assume any specific language, framework, or build system unless the user adds it.
- Preserve existing documentation structure and versioning when editing triage docs.
- When creating new files, keep them in the `docs/` folder unless the user explicitly requests otherwise.
- Ask the user before initializing a project scaffold, since the repository currently has no codebase conventions.

## Useful links
- `README.md` — repository landing page
- `docs/prd.md` — current PRD draft created during this session
- `docs/workflow-platform-prd.md` — earlier workflow platform PRD notes
- `docs/diagrams/` — diagram sources (`mmd/`) and rendered images (`img/`)
