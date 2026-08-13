using WorkflowPlatform.Api.Domain;

namespace WorkflowPlatform.Api.Contracts;

/// <summary>
/// Wire payload for creating a <see cref="WorkflowDefinition"/> (or a new version of one). Node
/// <see cref="CreateNodeRequest.Id"/> values are client-supplied so that <see cref="CreateConnectionRequest"/>
/// entries can reference sibling nodes within the same payload before either is persisted.
/// </summary>
public class CreateWorkflowDefinitionRequest
{
    public required string Name { get; set; }

    public required string CreatedBy { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? Metadata { get; set; }

    public List<CreateNodeRequest> Nodes { get; set; } = new();

    public List<CreateConnectionRequest> Connections { get; set; } = new();
}

public class CreateNodeRequest
{
    public required Guid Id { get; set; }

    public required NodeType Type { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? Configuration { get; set; }
}

public class CreateConnectionRequest
{
    public required Guid SourceNodeId { get; set; }

    public required Guid TargetNodeId { get; set; }

    public string? ConditionExpression { get; set; }
}
