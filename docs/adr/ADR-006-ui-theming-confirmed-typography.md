# ADR-006: UI Theming — Custom Angular Material Theme with Licensed Catalog Regular Typeface

> Status: ACCEPTED
> Date: 2026-08-12
> Architecture: docs/architecture.md (v3)

## Context
ADR-005 established a custom Angular Material theme from the Shaw and Partners brand guidelines (Shaw Orange primary, Black/White/55%-grey neutrals, secondary accent palette), but left the headline typeface, Catalog Regular, as an open risk — it appeared to be a licensed/proprietary typeface not confirmed as freely embeddable on the web. That license is now confirmed available, so the font can be downloaded and self-hosted (or loaded from a licensed web font provider) rather than substituted with a fallback.

## Decision
Build the custom Angular Material theme exactly as ADR-005 specified (Shaw Orange primary, neutral and secondary palettes), and use the licensed Catalog Regular webfont directly for headlines/subheads, with Helvetica Neue (its licensed weights) for body/supporting text — no open-source fallback needed.

## Alternatives Considered

| Option | Why not chosen |
|--------|-----------------|
| Keep Helvetica Neue / open equivalent (Inter) as a permanent headline substitute (ADR-005's interim approach) | No longer necessary now that the Catalog Regular license is confirmed available |
| Load fonts from a third-party CDN without self-hosting | Self-hosting/downloading the licensed font files avoids depending on external CDN availability and keeps license terms under the team's control |

## Consequences
- Gains: full, on-brand typography as specified in the brand guidelines, with no licensing risk outstanding.
- Tradeoff: font files (Catalog Regular, Helvetica Neue weights) must be added to the Angular project's assets and declared via `@font-face`, increasing initial bundle/asset size slightly versus using a system/open font — acceptable for POC scope.

## Related
- Architecture section: §3 Components — Angular Web Client
- Supersedes: ADR-005
