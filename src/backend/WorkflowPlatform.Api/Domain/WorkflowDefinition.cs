namespace WorkflowPlatform.Api.Domain;

/// <summary>
/// A versioned workflow definition (the authored node graph). Per NFR-005, <see cref="Version"/>
/// is independently incrementable — a new row is created for each version rather than mutating
/// an existing one, so <see cref="WorkflowInstance.DefinitionVersion"/> can safely snapshot it.
/// </summary>
public class WorkflowDefinition
{
    public Guid DefinitionId { get; set; }

    public required string Name { get; set; }

    public int Version { get; set; }

    public string? Description { get; set; }

    /// <summary>
    /// Free-form status label. Architecture §4 lists this field without cross-referencing a
    /// lifecycle diagram (unlike Node/WorkflowInstance), so no closed enum is defined here.
    /// </summary>
    public string? Status { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Free-form key/value or JSON metadata; no schema is defined for this in Architecture §4.
    /// </summary>
    public string? Metadata { get; set; }

    public ICollection<Node> Nodes { get; set; } = new List<Node>();

    public ICollection<Connection> Connections { get; set; } = new List<Connection>();
}
