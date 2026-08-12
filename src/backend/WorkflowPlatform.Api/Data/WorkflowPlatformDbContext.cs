using Microsoft.EntityFrameworkCore;

namespace WorkflowPlatform.Api.Data;

// Infrastructure-only for F-015; domain DbSet<T> properties are added in F-001.
public class WorkflowPlatformDbContext : DbContext
{
    public WorkflowPlatformDbContext(DbContextOptions<WorkflowPlatformDbContext> options)
        : base(options)
    {
    }
}
