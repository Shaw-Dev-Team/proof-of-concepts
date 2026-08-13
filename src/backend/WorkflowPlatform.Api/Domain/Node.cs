namespace WorkflowPlatform.Api.Domain;

/// <summary>
/// A node within a <see cref="WorkflowDefinition"/>'s graph. Per ADR-003, incoming/outgoing
/// adjacency is exposed via normalized <see cref="Connection"/> navigations rather than
/// denormalized ID arrays on this entity.
/// </summary>
/// <remarks>
/// Keyed by the composite (<see cref="NodeId"/>, <see cref="WorkflowDefinitionId"/>), not
/// <see cref="NodeId"/> alone, so a node's logical identity can be preserved across versions
/// (the client may reuse a v1 <see cref="NodeId"/> in a v2 payload) while every version's row
/// tree remains a fully independent, immutable copy per NFR-005.
/// </remarks>
public class Node
{
    public Guid NodeId { get; set; }

    public Guid WorkflowDefinitionId { get; set; }

    public NodeType Type { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    /// <summary>
    /// Free-form per-node-type configuration payload (JSON or similar); no fixed schema is
    /// defined for this in Architecture §4.
    /// </summary>
    public string? Configuration { get; set; }

    public WorkflowDefinition? WorkflowDefinition { get; set; }

    public ICollection<Connection> IncomingConnections { get; set; } = new List<Connection>();

    public ICollection<Connection> OutgoingConnections { get; set; } = new List<Connection>();
}
