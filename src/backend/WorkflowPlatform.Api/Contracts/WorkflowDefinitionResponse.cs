using WorkflowPlatform.Api.Domain;

namespace WorkflowPlatform.Api.Contracts;

/// <summary>
/// Full wire representation of a <see cref="WorkflowDefinition"/> including its node/connection graph.
/// </summary>
public record WorkflowDefinitionResponse(
    Guid DefinitionId,
    string Name,
    int Version,
    string? Description,
    string? Status,
    string CreatedBy,
    DateTime CreatedAt,
    string? Metadata,
    IReadOnlyList<NodeResponse> Nodes,
    IReadOnlyList<ConnectionResponse> Connections);

/// <summary>
/// Summary wire representation of a <see cref="WorkflowDefinition"/> without its graph — used for
/// listing endpoints where the full node/connection payload isn't needed.
/// </summary>
public record WorkflowDefinitionSummaryResponse(
    Guid DefinitionId,
    string Name,
    int Version,
    string? Description,
    string? Status,
    string CreatedBy,
    DateTime CreatedAt);

public record NodeResponse(Guid Id, NodeType Type, string Name, string? Description, string? Configuration);

public record ConnectionResponse(Guid Id, Guid SourceNodeId, Guid TargetNodeId, string? ConditionExpression);
