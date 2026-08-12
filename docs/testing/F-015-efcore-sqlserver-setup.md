# Test Runbook: F-015 — SQL Server 2022 / EF Core Connection Setup

> Feature: SQL Server 2022 / EF Core connection setup (DbContext skeleton, connection string configuration)
> Phase: 0
> Status: Done

## Automated Tests

None — this feature's own verification mechanism is the EF Core CLI tooling (migrations add/update), not unit tests, per its Test Expectations.

## Manual Verification

### 1. `DbContext` is registered in DI

- Confirm [Program.cs](../../src/backend/WorkflowPlatform.Api/Program.cs) calls `builder.Services.AddDbContext<WorkflowPlatformDbContext>(...)`.
- Confirm [Data/WorkflowPlatformDbContext.cs](../../src/backend/WorkflowPlatform.Api/Data/WorkflowPlatformDbContext.cs) defines an empty `WorkflowPlatformDbContext : DbContext` (domain `DbSet<T>` properties are added later, in F-001).

### 2. Connection string is read from configuration, not hardcoded

- Confirm `Program.cs` reads the connection string via `builder.Configuration.GetConnectionString("WorkflowPlatformDb")`.
- Confirm [appsettings.json](../../src/backend/WorkflowPlatform.Api/appsettings.json) defines the `ConnectionStrings:WorkflowPlatformDb` value — no connection string literal appears inline in code.

### 3. EF Core migrations tooling builds and connects

```powershell
cd src/backend\WorkflowPlatform.Api
dotnet ef migrations add InitialEmpty
```

- Expected: succeeds and generates a migration under [Migrations/](../../src/backend/WorkflowPlatform.Api/Migrations/) (already present as `20260812223313_InitialEmpty`) — confirms the EF Core tooling can build the project and resolve the `DbContext`.

### 4. Migrations apply to a real database — **verified against LocalDB**

```powershell
cd src/backend\WorkflowPlatform.Api
dotnet ef database update
```

- Expected: the `InitialEmpty` migration applies cleanly against `(localdb)\MSSQLLocalDB` using the connection string configured in `appsettings.Development.json`. Verified for real — see PMBook PM-001 (resolved).
- A full SQL Server 2022 Database Engine is still not installed on this machine; only LocalDB has been verified. Re-verify against a real SQL Server 2022 instance if/when one becomes available.

### 5. Database and migrations apply automatically on startup — **no manual step required**

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "DROP DATABASE IF EXISTS WorkflowPlatformDb;" -C
cd src/backend\WorkflowPlatform.Api
dotnet run
```

- Expected: on startup, [Program.cs](../../src/backend/WorkflowPlatform.Api/Program.cs) resolves `WorkflowPlatformDbContext` from a DI scope and calls `Database.Migrate()` before the app starts serving requests. The log output shows `CREATE DATABASE [WorkflowPlatformDb];` followed by `Applying migration '20260812223313_InitialEmpty'`, with no manual `dotnet ef database update` needed.
- Confirm afterward via `sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT name FROM sys.databases WHERE name = 'WorkflowPlatformDb';" -C` that the database exists, and that `__EFMigrationsHistory` contains `20260812223313_InitialEmpty`.
- Do not re-attempt this step as a pass/fail check until PM-001 is resolved (a real SQL Server 2022 instance installed and reachable at the configured connection string). Once resolved, re-run this step and update this runbook to reflect a passing result.

## Notes

- Steps 1–3 are fully verifiable today. Step 4 is documented as a manual verification step but is blocked on PM-001 and must not be reported as passing until that dependency is resolved.
