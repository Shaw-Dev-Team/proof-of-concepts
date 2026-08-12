# Test Runbook: F-014 — ASP.NET Core Web API Scaffolding

> Feature: ASP.NET Core Web API scaffolding (.NET 10 LTS solution/project, EF Core + SQL Server provider references)
> Phase: 0
> Status: Done

## Automated Tests

None — this feature is scaffolding only, with no business logic to unit test, per its own Test Expectations.

## Manual Verification

### 1. Solution builds

```powershell
cd src/backend
dotnet build WorkflowPlatform.slnx
```

- Expected: build succeeds with no errors.

### 2. API runs and serves the default endpoint

```powershell
cd src/backend\WorkflowPlatform.Api
dotnet run
```

- Expected: the app starts and reports a listening URL (see [Properties/launchSettings.json](../../src/backend/WorkflowPlatform.Api/Properties/launchSettings.json) for the configured port/profile).
- With the app running, call the default scaffolded endpoint:

```powershell
curl http://localhost:<port>/weatherforecast
```

- Expected: a JSON array of 5 forecast objects (`date`, `temperatureC`, `summary`, `temperatureF`), as defined in [Program.cs](../../src/backend/WorkflowPlatform.Api/Program.cs).

### 3. Required package references are present

- Confirm [WorkflowPlatform.Api.csproj](../../src/backend/WorkflowPlatform.Api/WorkflowPlatform.Api.csproj) references `Microsoft.EntityFrameworkCore` and `Microsoft.EntityFrameworkCore.SqlServer`, per ADR-002 and ADR-003.

## Notes

- No breaking or regression-sensitive flows apply to this scaffolding-only feature.
