namespace WorkflowPlatform.Api.Domain;

/// <summary>
/// A single execution-history entry for one node within a <see cref="WorkflowInstance"/>.
/// </summary>
public class NodeExecution
{
    public Guid EventId { get; set; }

    public Guid WorkflowInstanceId { get; set; }

    public Guid NodeId { get; set; }

    /// <summary>
    /// The <see cref="WorkflowDefinition"/> version that <see cref="NodeId"/> belongs to — required
    /// alongside <see cref="NodeId"/> to resolve <see cref="Node"/>'s composite key (a node's identity
    /// is only unique per definition version, not globally).
    /// </summary>
    public Guid WorkflowDefinitionId { get; set; }

    public NodeExecutionState State { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Free-form outcome/result payload; no fixed schema is defined for this in Architecture §4.
    /// </summary>
    public string? Result { get; set; }

    /// <summary>
    /// Populated once a Condition/Switch node's evaluation moves from Pending to Evaluated
    /// (Architecture §4 "Condition Evaluation State"); null for non-branching nodes.
    /// </summary>
    public string? EvaluationOutcome { get; set; }

    public WorkflowInstance? WorkflowInstance { get; set; }

    public Node? Node { get; set; }
}
