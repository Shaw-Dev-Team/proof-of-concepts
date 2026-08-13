namespace WorkflowPlatform.Api.Domain;

/// <summary>
/// A directed edge between two <see cref="Node"/>s within a <see cref="WorkflowDefinition"/>.
/// </summary>
public class Connection
{
    public Guid ConnectionId { get; set; }

    public Guid WorkflowDefinitionId { get; set; }

    public Guid SourceNodeId { get; set; }

    public Guid TargetNodeId { get; set; }

    /// <summary>
    /// Optional expression evaluated for Condition/Switch nodes; null for other connection types.
    /// </summary>
    public string? ConditionExpression { get; set; }

    public WorkflowDefinition? WorkflowDefinition { get; set; }

    public Node? SourceNode { get; set; }

    public Node? TargetNode { get; set; }
}
