using Microsoft.EntityFrameworkCore;
using WorkflowPlatform.Api.Domain;

namespace WorkflowPlatform.Api.Data;

public class WorkflowPlatformDbContext : DbContext
{
    public WorkflowPlatformDbContext(DbContextOptions<WorkflowPlatformDbContext> options)
        : base(options)
    {
    }

    public DbSet<WorkflowDefinition> WorkflowDefinitions => Set<WorkflowDefinition>();

    public DbSet<Node> Nodes => Set<Node>();

    public DbSet<Connection> Connections => Set<Connection>();

    public DbSet<WorkflowInstance> WorkflowInstances => Set<WorkflowInstance>();

    public DbSet<NodeExecution> NodeExecutions => Set<NodeExecution>();

    public DbSet<TaskHandlerReference> TaskHandlerReferences => Set<TaskHandlerReference>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkflowDefinition>(entity =>
        {
            entity.HasKey(d => d.DefinitionId);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(256);
            entity.Property(d => d.CreatedBy).IsRequired().HasMaxLength(256);
            entity.HasIndex(d => new { d.Name, d.Version }).IsUnique();

            entity.HasMany(d => d.Nodes)
                .WithOne(n => n.WorkflowDefinition)
                .HasForeignKey(n => n.WorkflowDefinitionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.Connections)
                .WithOne(c => c.WorkflowDefinition)
                .HasForeignKey(c => c.WorkflowDefinitionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Node>(entity =>
        {
            // Composite so a node's logical identity (NodeId) can be reused across versions
            // (WorkflowDefinitionId) without colliding \u2014 see Node's remarks.
            entity.HasKey(n => new { n.NodeId, n.WorkflowDefinitionId });
            entity.Property(n => n.Name).IsRequired().HasMaxLength(256);

            entity.HasMany(n => n.OutgoingConnections)
                .WithOne(c => c.SourceNode)
                .HasForeignKey(c => new { c.SourceNodeId, c.WorkflowDefinitionId })
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(n => n.IncomingConnections)
                .WithOne(c => c.TargetNode)
                .HasForeignKey(c => new { c.TargetNodeId, c.WorkflowDefinitionId })
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Connection>(entity =>
        {
            entity.HasKey(c => c.ConnectionId);
        });

        modelBuilder.Entity<WorkflowInstance>(entity =>
        {
            entity.HasKey(i => i.InstanceId);

            entity.HasOne(i => i.WorkflowDefinition)
                .WithMany()
                .HasForeignKey(i => i.WorkflowDefinitionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(i => i.ExecutionHistory)
                .WithOne(e => e.WorkflowInstance)
                .HasForeignKey(e => e.WorkflowInstanceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Explicit shared-type join entity (rather than the implicit anonymous-type join
            // previously used) because Node's key is now composite \u2014 the join table must carry
            // WorkflowDefinitionId alongside NodeId for the FK to Node to resolve.
            entity.HasMany(i => i.CurrentNodes)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "WorkflowInstanceCurrentNodes",
                    j => j.HasOne<Node>().WithMany().HasForeignKey("NodeId", "WorkflowDefinitionId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<WorkflowInstance>().WithMany().HasForeignKey("WorkflowInstanceId").OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("WorkflowInstanceId", "NodeId", "WorkflowDefinitionId");
                        j.ToTable("WorkflowInstanceCurrentNodes");
                    });
        });

        modelBuilder.Entity<NodeExecution>(entity =>
        {
            entity.HasKey(e => e.EventId);

            entity.HasOne(e => e.Node)
                .WithMany()
                .HasForeignKey(e => new { e.NodeId, e.WorkflowDefinitionId })
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TaskHandlerReference>(entity =>
        {
            entity.HasKey(t => t.TaskHandlerReferenceId);
            entity.Property(t => t.HandlerType).IsRequired().HasMaxLength(256);

            entity.HasOne(t => t.Node)
                .WithOne()
                .HasForeignKey<TaskHandlerReference>(t => new { t.NodeId, t.WorkflowDefinitionId })
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(t => new { t.NodeId, t.WorkflowDefinitionId }).IsUnique();
        });
    }
}
