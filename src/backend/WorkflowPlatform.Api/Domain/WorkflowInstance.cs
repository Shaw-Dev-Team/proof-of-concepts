namespace WorkflowPlatform.Api.Domain;

/// <summary>
/// A running/completed instance of a <see cref="WorkflowDefinition"/>.
/// </summary>
public class WorkflowInstance
{
    public Guid InstanceId { get; set; }

    public Guid WorkflowDefinitionId { get; set; }

    /// <summary>
    /// Snapshot of the definition's <see cref="WorkflowDefinition.Version"/> at instance-creation
    /// time — deliberately not a live FK to a mutable row, per NFR-005 / Acceptance Criterion 5.
    /// </summary>
    public int DefinitionVersion { get; set; }

    public WorkflowInstanceStatus Status { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public WorkflowDefinition? WorkflowDefinition { get; set; }

    /// <summary>
    /// The set of nodes currently executing/active for this instance (supports concurrent
    /// branches from Parallel Split). Modeled as a many-to-many join, per ADR-003's normalized
    /// adjacency preference over a denormalized ID array.
    /// </summary>
    public ICollection<Node> CurrentNodes { get; set; } = new List<Node>();

    public ICollection<NodeExecution> ExecutionHistory { get; set; } = new List<NodeExecution>();
}
