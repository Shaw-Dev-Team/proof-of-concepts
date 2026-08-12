# ADR-005: UI Theming — Custom Angular Material Theme from Shaw and Partners Brand Guidelines

> Status: SUPERSEDED BY ADR-006
> Date: 2026-08-12
> Architecture: docs/architecture.md (v1)

## Context
The user provided the Shaw and Partners brand guidelines document and asked the frontend to follow it. Key extracted guidance: primary brand color "Shaw Orange" (Pantone 173C / CMYK 0-82-100-0 / RGB 232-72-16), a Black / Black 55% (grey) / White neutral set, a secondary accent palette (navy, teal, olive, yellow, orange), and two typefaces — Catalog Regular (headlines/brandmark) and Helvetica Neue (body/supporting text, multiple weights).

## Decision
Build a custom Angular Material theme using Shaw Orange as the primary palette color, Black/White/55%-grey as neutrals, and the secondary palette as limited accent colors — applied via Angular Material's theming APIs rather than overriding component styles ad hoc.

## Alternatives Considered

| Option | Why not chosen |
|--------|-----------------|
| Default Angular Material theme (e.g. Indigo/Pink) | Does not reflect the brand at all |
| Hand-rolled CSS overrides per component | Fragile, drifts from Material's own theming system, harder to maintain than a proper theme definition |

## Consequences
- Gains: consistent, brand-aligned look across all Material components without per-component overrides.
- Tradeoff / open risk: Catalog Regular appears to be a licensed/proprietary typeface tied to the brandmark, not confirmed as freely licensed for web embedding. Until licensing is confirmed, use Helvetica Neue (or a close open web-safe equivalent, e.g. Roboto/Inter for body text) and treat Catalog Regular as a headline-only, later-confirmed addition. Tracked in Architecture §8 (Open Questions & Risks, A3).

## Related
- Architecture section: §3 Components — Angular Web Client
- Supersedes: none
