namespace WorkflowPlatform.Api.Domain;

/// <summary>
/// Associates a Task-type <see cref="Node"/> with the pluggable <c>ITaskHandler</c> (ADR-004)
/// that will be invoked when the node executes.
/// </summary>
public class TaskHandlerReference
{
    public Guid TaskHandlerReferenceId { get; set; }

    public Guid NodeId { get; set; }

    /// <summary>
    /// The <see cref="WorkflowDefinition"/> version that <see cref="NodeId"/> belongs to — required
    /// alongside <see cref="NodeId"/> to resolve <see cref="Node"/>'s composite key (a node's identity
    /// is only unique per definition version, not globally).
    /// </summary>
    public Guid WorkflowDefinitionId { get; set; }

    /// <summary>
    /// Identifies the registered <c>ITaskHandler</c> implementation (e.g. "SendEmail",
    /// "CreateInvoice"); deliberately a string, not an enum, since handlers are pluggable (ADR-004).
    /// </summary>
    public required string HandlerType { get; set; }

    public string? Configuration { get; set; }

    public string? ExternalMetadata { get; set; }

    public Node? Node { get; set; }
}
