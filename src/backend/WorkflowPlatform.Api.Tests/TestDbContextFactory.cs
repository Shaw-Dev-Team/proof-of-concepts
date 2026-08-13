using Microsoft.EntityFrameworkCore;
using WorkflowPlatform.Api.Data;

namespace WorkflowPlatform.Api.Tests;

/// <summary>
/// Uses the EF Core in-memory provider (a unique database per test) rather than a real
/// LocalDB integration test — sufficient for CRUD/relationship/nullability verification
/// and keeps the test suite runnable without a LocalDB dependency.
/// </summary>
internal static class TestDbContextFactory
{
    public static WorkflowPlatformDbContext Create()
    {
        var options = new DbContextOptionsBuilder<WorkflowPlatformDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new WorkflowPlatformDbContext(options);
    }
}
