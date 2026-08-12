# Test Runbook: F-013 — Angular Workspace Scaffolding

> Feature: Angular workspace scaffolding (Angular CLI latest, standalone app, Angular Material installed)
> Phase: 0
> Status: Done

## Automated Tests

### Default Angular CLI smoke test (root app component)

The scaffolding-generated spec at [src/frontend/src/app/app.spec.ts](../../src/frontend/src/app/app.spec.ts) verifies:

- The root `App` component compiles and bootstraps without error (`should create the app`).
- The rendered template contains the expected title text inside a `mat-toolbar` element (`should render title`), confirming Angular Material is installed and wired into the standalone app.

**Run:**

```powershell
cd src/frontend
npm test
```

**Expected result:** both tests pass; no compilation or bootstrap errors.

## Manual Verification

### 1. Dev server serves the app

```powershell
cd src/frontend
npm start
```

- Expected: `ng serve` starts without errors and reports a local URL (e.g. `http://localhost:4200/`).
- Open the URL in a browser; the page loads without console errors and the Angular Material toolbar is visible.

### 2. Production build succeeds

```powershell
cd src/frontend
npm run build
```

- Expected: build completes without errors and emits output assets.

## Notes

- No feature-specific business-logic tests are expected for this item — coverage is limited to the CLI-generated smoke test and manual serve/build verification, per this feature's Test Expectations.
