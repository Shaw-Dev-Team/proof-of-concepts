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
            entity.HasKey(n => n.NodeId);
            entity.Property(n => n.Name).IsRequired().HasMaxLength(256);

            entity.HasMany(n => n.OutgoingConnections)
                .WithOne(c => c.SourceNode)
                .HasForeignKey(c => c.SourceNodeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(n => n.IncomingConnections)
                .WithOne(c => c.TargetNode)
                .HasForeignKey(c => c.TargetNodeId)
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

            entity.HasMany(i => i.CurrentNodes)
                .WithMany()
                .UsingEntity(j => j.ToTable("WorkflowInstanceCurrentNodes"));
        });

        modelBuilder.Entity<NodeExecution>(entity =>
        {
            entity.HasKey(e => e.EventId);

            entity.HasOne(e => e.Node)
                .WithMany()
                .HasForeignKey(e => e.NodeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TaskHandlerReference>(entity =>
        {
            entity.HasKey(t => t.TaskHandlerReferenceId);
            entity.Property(t => t.HandlerType).IsRequired().HasMaxLength(256);

            entity.HasOne(t => t.Node)
                .WithOne()
                .HasForeignKey<TaskHandlerReference>(t => t.NodeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(t => t.NodeId).IsUnique();
        });
    }
}
